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


        event EventHandler ChangesMadeToSelectedStation;

        event EventHandler SelectedStationChanged;

        event EventHandler RecordsUpdatedForSelectedStation;

        void OnSelectedStationChanged();

        void OnRecordsUpdatedForSelectedStation();
        void OnChangesMadeToSelectedStation();


        void OnGetRecordsStarted();
        void OnGetRecordsCompleted();
    }
}