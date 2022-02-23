#if WINUI
using Microsoft.UI.Xaml.Controls;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
#endif

namespace DRE.Views
{
    public sealed partial class ShellView : ContentDialog
    {
        public ShellView()
        {
            this.InitializeComponent();
        }
    }
}
