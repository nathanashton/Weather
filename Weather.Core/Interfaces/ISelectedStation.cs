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
        string TimeSpanWords { get;  }

        event EventHandler StationsChanged;

        event EventHandler SelectedStationsChanged;

        event EventHandler TimeSpanChanged;

        void OnStationsChanged();

        void OnSelectedStationChanged();

        void OnTimeSpanChanged();

        void BackOnePeriod();

        void ForwardOnePeriod();

    }
}