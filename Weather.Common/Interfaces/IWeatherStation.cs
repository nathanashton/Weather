using System.Collections.Generic;
using System.Collections.ObjectModel;
using Weather.Common.Entities;

namespace Weather.Common.Interfaces
{
    public interface IWeatherStation
    {
        long Id { get; set; }
        string Manufacturer { get; set; }
        string Model { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
        ObservableCollection<IWeatherRecord> WeatherRecords { get; set; }
        ObservableCollection<ISensor> Sensors { get; set; }

        void AddRecord(IWeatherRecord record);
        void AddSensor(ISensor sensor);

        ICollection<IWeatherRecordSingleSensor> GetValuesForSensorType(Enums.UnitType sensorType);
        //ObservableCollection<ISensorValue> AllSensorValues { get; set; }
        //ObservableCollection<SensorDataGridRow> SensorDataGridRows { get; set; }
        //void RefreshSensorValues();

        //void AddSensor(ISensor sensor);
    }
}