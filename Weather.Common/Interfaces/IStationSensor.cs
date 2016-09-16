namespace Weather.Common.Interfaces
{
    public interface IStationSensor
    {
        long StationSensorId { get; set; }
        ISensor Sensor { get; set; }

        double Correction { get; set; }
        string Notes { get; set; }
    }
}