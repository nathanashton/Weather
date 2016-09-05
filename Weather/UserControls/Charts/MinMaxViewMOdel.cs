using PropertyChanged;
using System;
using System.Windows;
using Weather.Common;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;

namespace Weather.UserControls.Charts
{
    [ImplementPropertyChanged]
    public class MinMaxViewModel : NotifyBase
    {
        public string Header => "Minimum / Maximum";

        public ISelectedStation SelectedStation { get; set; }
        public IStationSensor SelectedSensor { get; set; }
        
        public string Title
        {
            get
            {
                if (SelectedSensor == null)
                {
                    return "Min / Max (" + SelectedStation.TimeSpanWords + ")";
                }
                else
                {
                    return "Min / Max " + SelectedSensor.Sensor.SensorType + " (" + SelectedStation.TimeSpanWords + ")";
                }
            }
        }



        public MinMaxViewModel(ISelectedStation selectedStation)
        {
            SelectedStation = selectedStation;
            SelectedStation.SelectedStationsChanged += SelectedStation_SelectedStationsChanged;
            SelectedStation.TimeSpanChanged += SelectedStation_TimeSpanChanged;
        }

        private void SelectedStation_TimeSpanChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(()=> Title);
        }

        private void SelectedStation_SelectedStationsChanged(object sender, EventArgs e)
        {
        }

     }
}