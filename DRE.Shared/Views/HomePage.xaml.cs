#if WINUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
#endif

namespace DRE
{
	public sealed partial class HomePage : Page
    {
        public HomePage() => InitializeComponent();
        private void Resize_Nav_Panel(object sender, RoutedEventArgs e) => NavDRE.Width = NavDRE.ActualWidth > 48 ? 48 : NavDREWideWidth;
     
        private void ToggleTheme(object sender, RoutedEventArgs e) => 
            RequestedTheme = RequestedTheme == ElementTheme.Dark ? ElementTheme.Light : ElementTheme.Dark;
      
        private double NavDREWideWidth { get; set; }

        private void Page_Loaded(object sender, RoutedEventArgs e) => NavDREWideWidth = NavDRE.ActualWidth;
       
    }
}
