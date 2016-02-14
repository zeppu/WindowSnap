using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Overlay.ViewModels;

namespace Overlay.Views
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
