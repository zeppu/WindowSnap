using System.Windows;
using Snapinator.ViewModels;

namespace Snapinator.Views
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
