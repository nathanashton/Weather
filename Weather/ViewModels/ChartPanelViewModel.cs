using PropertyChanged;
using System;
using System.Windows;
using System.Windows.Controls;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.UserControls;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class ChartPanelViewModel
    {
        public ChartPanel Window { get; set; }
        public ISelectedStation Station { get; set; }
        public IStationSensor SelectedStationSensorTwo { get; set; }
        public IStationSensor SelectedStationSensorOne { get; set; }

        public ChartPanelViewModel(ISelectedStation station)
        {
            Station = station;
            Station.StationsChanged += Station_StationsChanged;
        }

        private void Station_StationsChanged(object sender, EventArgs e)
        {

        }
    }
}