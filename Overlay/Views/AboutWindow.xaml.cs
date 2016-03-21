using System.Windows;
using Overlay.ViewModels;

namespace Overlay.Views
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow(IAboutViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
