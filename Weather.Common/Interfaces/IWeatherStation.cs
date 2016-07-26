using System.Collections.Generic;
using Weather.Common.Entities;

namespace Weather.Common.Interfaces
{
    public interface IWeatherStation
    {
        int Id { get; set; }
        string Manufacturer { get; set; }
        string Model { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
        ICollection<IWeatherRecord> WeatherRecords { get; set; }
        ICollection<ISensor> Sensors { get; set; }

        void AddRecord(IWeatherRecord record);

        ICollection<IWeatherRecordSingleSensor> GetValuesForSensorType(Enums.UnitType sensorType);
        //ObservableCollection<ISensorValue> AllSensorValues { get; set; }
        //ObservableCollection<SensorDataGridRow> SensorDataGridRows { get; set; }
        //void RefreshSensorValues();

        //void AddSensor(ISensor sensor);
    }
}