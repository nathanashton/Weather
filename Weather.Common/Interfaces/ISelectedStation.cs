using System;

namespace Weather.Common.Interfaces
{
    public interface ISelectedStation
    {
        IWeatherStation WeatherStation { get; set; }



        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }




        event EventHandler GetRecordsStarted;
        event EventHandler GetRecordsCompleted;

        event EventHandler SelectedStationUpdated;
        event EventHandler SelectedStationChanged;
        event EventHandler SelectedStationRecordsUpdated;

        void OnSelectedStationRecordsUpdated();
        void OnSelectedStationChanged();
        void OnSelectedStationUpdated();


        void OnGetRecordsStarted();
        void OnGetRecordsCompleted();


    }
}