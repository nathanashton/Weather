using System;
using Weather.Common.Interfaces;

namespace Weather.Core.Interfaces
{
    public interface ISelectedStation
    {
        IWeatherStation WeatherStation { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }

        bool TimeSpanDay { get; set; }
        bool TimeSpanWeek { get; set; }
        bool TimeSpanMonth { get; set; }
        bool TimeSpanYear { get; set; }
        TimeSpan TimeSpan { get; set; }
        string TimeSpanWords { get; }

        event EventHandler GetRecordsStarted;
        event EventHandler GetRecordsCompleted;

        event EventHandler SelectedStationChanged;
        event EventHandler SelectedStationUpdated;

        void OnSelectedStationUpdated();
        void OnSelectedStationChanged();

        void OnGetRecordsStarted();
        void OnGetRecordsCompleted();


        void BackOnePeriod();

        void ForwardOnePeriod();

        void SetTimeSpanDay();

        void SetTimeSpanWeek();

        void SetTimeSpanMonth();

        void SetTimeSpanYear();
    }
}