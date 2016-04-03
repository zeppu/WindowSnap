using System.Windows;
using Snapinator.ViewModels;

namespace Snapinator.Views
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        public ConfigurationWindow(IConfigurationViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
