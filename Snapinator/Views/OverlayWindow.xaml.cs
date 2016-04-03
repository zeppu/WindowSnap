using System.Windows;
using Snapinator.ViewModels;

namespace Snapinator.Views
{
    /// <summary>
    /// Interaction logic for OverlayWindow.xaml
    /// </summary>
    public partial class OverlayWindow : Window
    {
        public OverlayWindow(IOverlayViewModel model)
        {
            InitializeComponent();

            DataContext = model;
        }

        private void OverlayWindow_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
