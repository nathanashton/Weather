using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;

namespace Weather.UserControls.Charts
{
    [ImplementPropertyChanged]
    public class AllRecordsViewModel
    {
        private readonly IWeatherRecordCore _weatherRecordCore;
        public string Header => "All Records";
        public ObservableCollection<IWeatherRecord> Records { get; set; }
        public ISelectedStation SelectedStation { get; set; }
        public AllRecords Window { get; set; }

        public AllRecordsViewModel(ISelectedStation selectedStation, IWeatherRecordCore weatherRecordCore)
        {
            _weatherRecordCore = weatherRecordCore;
            SelectedStation = selectedStation;
        }

        public void SelectedStation_TimeSpanChanged(object sender, EventArgs e)
        {
            Draw();
        }

        public void SelectedStation_SelectedStationsChanged(object sender, EventArgs e)
        {
            Draw();
        }

        public void Draw()
        {
            if (SelectedStation?.WeatherStation == null)
            {
                return;
            }

            //if (SelectedStation.WeatherStation.Records == null || SelectedStation.WeatherStation.Records.Count == 0)
            //{
            //    SelectedStation.WeatherStation.Records =
            //        new ObservableCollection<IWeatherRecord>(
            //            _weatherRecordCore.GetAllRecordsForStationBetweenDates(
            //                SelectedStation.WeatherStation.WeatherStationId,
            //                SelectedStation.StartDate, SelectedStation.EndDate));
            //}
            Window.RenderGrid();
        }
    }
}