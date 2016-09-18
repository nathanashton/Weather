using System;
using System.Threading.Tasks;

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
        event EventHandlers.AsyncEventHandler SelectedStationRecordsUpdated;


        void OnSelectedStationRecordsUpdated();
        void OnSelectedStationChanged();
        void OnSelectedStationUpdated();


        void OnGetRecordsStarted();
        void OnGetRecordsCompleted();
    }

    public class EventHandlers
    {
        public delegate Task AsyncEventHandler(object sender, System.EventArgs e);
    }

}