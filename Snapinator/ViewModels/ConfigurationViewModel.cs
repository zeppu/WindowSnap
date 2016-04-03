using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Overlay.Core.Configuration;
using ReactiveUI;

namespace Overlay.ViewModels
{
    public class ConfigurationViewModel : ReactiveObject, IConfigurationViewModel
    {
        private readonly IConfigurationService _configurationService;
        public IList<IConfigurationPartViewModel> Parts { get; }

        public ReactiveCommand<object> SaveSettingsCommand { get; }

        public ConfigurationViewModel(IEnumerable<IConfigurationPartViewModel> parts, IConfigurationService configurationService)
        {
            _configurationService = configurationService;
            Parts = new ObservableCollection<IConfigurationPartViewModel>(parts);

            SaveSettingsCommand = ReactiveCommand.Create();
            SaveSettingsCommand.Subscribe(x =>
                {
                    foreach (var configurationPartViewModel in Parts)
                    {
                        configurationPartViewModel.CommitChanges();
                    }

                    configurationService.SaveConfiguration();
                });
        }
    }
}