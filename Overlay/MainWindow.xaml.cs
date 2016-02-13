using System.Windows;
using Overlay.ViewModels;

namespace Overlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(IMainViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}
