using System;
using System.Collections.ObjectModel;
using System.Windows;
using PropertyChanged;
using Weather.Common.Interfaces;
using Weather.Interfaces;

namespace Weather.Charts.AllRecords
{
    [ImplementPropertyChanged]
    public class AllRecordsViewModel : IChartViewModel
    {
        public ObservableCollection<IWeatherRecord> Records { get; set; }

        public Window Window { get; set; }

        public AllRecordsViewModel(ISelectedStation selectedStation)
        {
            SelectedStation = selectedStation;
        }

        public ISelectedStation SelectedStation { get; set; }

        public string Header => "All Records";

        public void SelectedStation_RecordsUpdated(object sender, EventArgs e)
        {
            DrawChart();
        }

        public void SelectedStation_GetRecordsCompleted(object sender, EventArgs e)
        {
            DrawChart();
        }

        public void SelectedStation_SelectedStationChanged(object sender, EventArgs e)
        {
            DrawChart();
        }

        public void DrawChart()
        {
            if (SelectedStation?.WeatherStation == null)
            {
            }
            //Draw
        }
    }
}