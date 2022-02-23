using System;
using System.Threading.Tasks;
using DRE.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Uno.Extensions.Configuration;
using Uno.Extensions.Hosting;
using Uno.Extensions.Logging;
using Uno.Extensions.Navigation;
using Uno.Extensions.Navigation.UI;
using Uno.Extensions.Navigation.Regions;
using Uno.Extensions.Navigation.Toolkit;
using Uno.Extensions.Serialization;
using DRE.Views;
using DRE.Interfaces;
using DRE.Services;
using DRE.Models;

#if WINUI
using Windows.ApplicationModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;
using Window = Microsoft.UI.Xaml.Window;
#else
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using LaunchActivatedEventArgs = Windows.ApplicationModel.Activation.LaunchActivatedEventArgs;
using Window = Windows.UI.Xaml.Window;
using CoreApplication = Windows.ApplicationModel.Core.CoreApplication;
#endif

namespace DRE
{
    public sealed partial class App : Application
    {
        private Window _window;
        public Window Window => _window;

        private IHost Host { get; }

        public App()
        {



            Host = UnoHost
                    .CreateDefaultBuilder(true)
#if DEBUG
                    // Switch to Development environment when running in DEBUG
                    .UseEnvironment(Environments.Development)
#endif


            // Add platform specific log providers
#if !HAS_UNO || __WASM__
            .UseLogging()
#else
			.UseLogging(b => b.AddSimpleConsole(options =>
			{
				options.ColorBehavior = Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Disabled;
			}))
#endif

                    // Configure log levels for different categories of logging
                    .ConfigureLogging(logBuilder =>
                    {
                        logBuilder
                                .SetMinimumLevel(LogLevel.Information)
                                .XamlLogLevel(LogLevel.Information)
                                .XamlLayoutLogLevel(LogLevel.Information);
                    })



                    // Load configuration information from appsettings.json
                    .UseAppSettings()

                    .UseEmbeddedAppSettings<App>(includeEnvironmentSettings: true)

                    .UseConfiguration<Config>()



                    // Register Json serializers (ISerializer and IStreamSerializer)
                    .UseSerialization()

                    // Register services for the application
                    .ConfigureServices(services =>
                    {
                        services

                        .AddSingleton<ISetupSvc, SetupSvc>()
                        .AddSingleton<ConfigSvc>();


                    })


                    // Enable navigation, including registering views and viewmodels
                    .UseNavigation(RegisterRoutes)

                    // Add navigation support for toolkit controls such as TabBar and NavigationView
                    .UseToolkitNavigation()


                    .Build(enableUnoLogging: true);




            this.InitializeComponent();

#if HAS_UNO || NETFX_CORE
			this.Suspending += OnSuspending;
#endif
        }


        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif


#if NET5_0 && WINDOWS
            _window = new Window();
            _window.Title = "DRE";
            _window.Activate();
#else
            _window = Window.Current;
#endif

            var notif = Host.Services.GetService<IRouteNotifier>();
            notif.RouteChanged += RouteUpdated;




            if (_window == null) _window = new Window();

            var setupSvc = Host.Services.GetService<ISetupSvc>();

            _window.Content = setupSvc.Setup ? new HomePage().WithNavigation(Host.Services) :
                                               new SetupDRE().WithNavigation(Host.Services);



            _window.Title = $"{_window.Bounds.Width} x {_window.Bounds.Height}";

            // _window.SizeChanged += TitleSize;
            _window.Activate();

            await Task.Run(async () =>
            {
                await Host.StartAsync();
            });

        }

        //private void TitleSize(object sender, WindowSizeChangedEventArgs args)
        //{

        //    //var c = Host.Services.GetService<IOptions<App>>();

        //    //_window.Title = c.Value.Title;

        //}

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new InvalidOperationException($"Failed to load {e.SourcePageType.FullName}: {e.Exception}");
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private static void RegisterRoutes(IRouteBuilder builder)
        {
            builder
                //.Register(RouteMap.For(nameof(ShellView)))
                //.Register(ViewMap.For(nameof(ShellView)).Show<ShellView>().With<ShellViewModel>())
                .Register(ViewMap.For("Home").Show<HomePage>().With<HomeViewModel>())
                .Register(ViewMap.For("Setup").Show<SetupDRE>().With<SetupDREViewModel>());


        }

        public async void RouteUpdated(object sender, EventArgs e)
        {
            try
            {
                var reg = Host.Services.GetService<IRegion>();
                var rootRegion = reg.Root();
                var route = rootRegion.GetRoute();


#if !__WASM__ && !WINUI
				CoreApplication.MainView?.DispatcherQueue.TryEnqueue(() =>
				{
					var appTitle = ApplicationView.GetForCurrentView();
					appTitle.Title = "DRE: " + (route + "").Replace("+", "/");
				});
#endif


#if __WASM__
				// Note: This is a hack to avoid error being thrown when loading products async
				await Task.Delay(1000).ConfigureAwait(false);
				CoreApplication.MainView?.DispatcherQueue.TryEnqueue(() =>
				{
					var href = WebAssemblyRuntime.InvokeJS("window.location.href");
					var url = new UriBuilder(href);
					url.Query = route.Query();
					url.Path = route.FullPath()?.Replace("+", "/");
					var webUri = url.Uri.OriginalString;
					var js = $"window.history.pushState(\"{webUri}\",\"\", \"{webUri}\");";
					Console.WriteLine($"JS:{js}");
					var result = WebAssemblyRuntime.InvokeJS(js);
				});
#endif
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
