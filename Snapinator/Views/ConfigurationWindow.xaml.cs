﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Overlay.ViewModels;

namespace Overlay.Views
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
