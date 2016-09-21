using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using PropertyChanged;
using Weather.Common;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.UserControls;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class MainViewModel : NotifyBase
    {
        private IWeatherStation _selected;

        private MenuStuff _selectedMenuStuff;

        public Blank StaticBlank { get; set; }
        public ObservableCollection<MenuStuff> MenuStuff { get; set; }

        public MenuStuff SelectedMenuStuff
        {
            get { return _selectedMenuStuff; }
            set
            {
                _selectedMenuStuff = value;
                OnPropertyChanged(() => SelectedMenuStuff);
                if (_selectedMenuStuff != null)
                {
                    Content = _selectedMenuStuff.Content;
                    SelectedStation.OnChangesMadeToSelectedStation();
                }
            }
        }

        public ISelectedStation SelectedStation { get; set; }

        public ContentControl Content { get; set; }


        public IWeatherStation Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                SelectedStation.WeatherStation = value;
                OnPropertyChanged(() => Selected);
                SelectedStation.OnSelectedStationChanged();
            }
        }


        public ObservableCollection<IWeatherStation> Stations { get; set; }

        public MainViewModel(ISelectedStation selected, IStationCore stationCore)
        {
            StaticBlank = new Blank();
            SelectedStation = selected;
            SelectedStation.SelectedStationChanged += SelectedStation_SelectedStationChanged;
            MenuStuff = new ObservableCollection<MenuStuff>
            {
                new MenuStuff {Name = "Main", Content = StaticBlank},
                new MenuStuff {Name = "Settings", Content = new Tabs()},
                new MenuStuff {Name = "Import", Content = new ImportControl()}
            };


            Stations = new ObservableCollection<IWeatherStation>();
            Stations = new ObservableCollection<IWeatherStation>(stationCore.GetAllStations());
            Content = StaticBlank;
        }

        private void SelectedStation_SelectedStationChanged(object sender, EventArgs e)
        {
            Selected = SelectedStation.WeatherStation;
        }
    }

    [ImplementPropertyChanged]
    public class MenuStuff
    {
        public string Name { get; set; }
        public ContentControl Content { get; set; }
    }
}