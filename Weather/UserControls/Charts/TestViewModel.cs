using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Weather.Common.Interfaces;
using Weather.Common.Units;
using Weather.Core.Interfaces;

namespace Weather.UserControls.Charts
{
    [ImplementPropertyChanged]
    public class TestViewModel
    {
        public TestViewModel(ISelectedStation selectedStation)
        {
            SelectedStation = selectedStation;
            Sensors = new ObservableCollection<ISensor>();
            SelectedStation.SelectedStationsChanged += SelectedStation_SelectedStationsChanged;

            Demands = new ObservableCollection<GoldDemand>
            {
                new GoldDemand
                {
                    Demand = "Jewelry",
                    Year2010 = 1998.0,
                    Year2011 = 2361.2
                },
                new GoldDemand {Demand = "Electronics", Year2010 = 1284.0, Year2011 = 1328.0},
                new GoldDemand {Demand = "Research", Year2010 = 1090.5, Year2011 = 1032.0},
                new GoldDemand {Demand = "Investment", Year2010 = 1643.0, Year2011 = 1898.0},
                new GoldDemand {Demand = "Bank Purchases", Year2010 = 987.0, Year2011 = 887.0}
            };
        }

        public ISelectedStation SelectedStation { get; set; }
        public ObservableCollection<ISensor> Sensors { get; set; }
        public string Header => "Average Wind Direction";
        public ObservableCollection<GoldDemand> Demands { get; set; }

        private void SelectedStation_SelectedStationsChanged(object sender, EventArgs e)
        {
            Test();
        }

        public void Test()
        {
            if (SelectedStation.WeatherStation == null) return;
            Sensors.Clear();
            var p = UnitTypes.UnitsList.First(x => x.Name == "Pressure");
            if (SelectedStation.WeatherStation.Sensors == null) return;
            foreach (var s in SelectedStation.WeatherStation.Sensors)
            {
                if (s.Sensor.SensorType.SIUnit.UnitType == p)
                {
                    Sensors.Add(s.Sensor);
                }
            }
        }
    }
}