using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using PropertyChanged;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.UserControls;
using Weather.Common;
using Weather.Core;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class TestViewModel : NotifyBase
    {
        public ObservableCollection<MenuStuff> MenuStuff { get;set; }

        private MenuStuff _selectedMenuStuff;

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
                }
            }
        }

        public ISelectedStation SelectedStation { get; set; }
        public ContentControl Content { get; set; }

        private IWeatherStation _selected;
        public IWeatherStation Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                SelectedStation.WeatherStation = value;
                SelectedStation.OnSelectedStationChanged();

              //  Dispatcher.CurrentDispatcher.InvokeAsync(async () => await GetRecords());

                OnPropertyChanged(() => Selected);
            }

        }



        public ObservableCollection<IWeatherStation> Stations { get; set; }

        public TestViewModel(ISelectedStation selected, IStationCore stationCore)
        {
            SelectedStation = selected;
            MenuStuff = new ObservableCollection<ViewModels.MenuStuff>
            {
                new ViewModels.MenuStuff {Name = "Main", Content=new Blank()},
                new ViewModels.MenuStuff {Name = "Settings", Content=new Tabs()},

            };


            Stations = new ObservableCollection<IWeatherStation>();
            Stations = new ObservableCollection<IWeatherStation>(stationCore.GetAllStations());
            if (Stations.Count == 1)
            {
                Selected = Stations.First();
            }
            Content = new Blank();
        }
    }

    [ImplementPropertyChanged]
    public class MenuStuff
    {
        public string Name { get; set; }
        public ContentControl Content { get; set; }
    }
}
