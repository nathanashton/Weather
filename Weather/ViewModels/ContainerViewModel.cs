using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using PropertyChanged;
using Weather.Common;
using Weather.Common.Interfaces;
using Weather.Helpers;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class ContainerViewModel : NotifyBase
    {
        private Chart _selected;
        public bool Loading { get; set; }
        public bool LoadingInvert { get; set; }

        public ObservableCollection<IPluginWrapper> Charts { get; set; }
        public List<MenuItem> MenuItems { get; set; }

        public Chart Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                Content = _selected.Content;
                OnPropertyChanged(() => Selected);
            }
        }

        public ContentControl Content { get; set; }
        public ISelectedStation SelectedStation { get; set; }

        public ICommand GraphSelected
        {
            get { return new RelayCommand(G, x => true); }
        }

        public ContainerViewModel(ISelectedStation selectedStation)
        {
            Loading = false;
            LoadingInvert = true;

            SelectedStation = selectedStation;
            SelectedStation.GetRecordsStarted += SelectedStation_GetRecordsStarted;
            SelectedStation.GetRecordsCompleted += SelectedStation_GetRecordsCompleted;

            if ((Program.LoadedPlugins == null) || (Program.LoadedPlugins.Count == 0))
            {
                return;
            }
            Charts = new ObservableCollection<IPluginWrapper>();
            foreach (var plugin in Program.LoadedPlugins)
            {
                Charts.Add(plugin);
            }
        }


        private void SelectedStation_GetRecordsCompleted(object sender, EventArgs e)
        {
            Loading = false;
            LoadingInvert = true;
        }

        private void SelectedStation_GetRecordsStarted(object sender, EventArgs e)
        {
            if (Content == null)
            {
                return;
            }
            Loading = true;
            LoadingInvert = false;
        }

        private void G(object obj)
        {
            var t = obj as UserControl;
            Content = t;
        }
    }

    public class Chart
    {
        public string Name { get; set; }
        public ContentControl Content { get; set; }
    }
}