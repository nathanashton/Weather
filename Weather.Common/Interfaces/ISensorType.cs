namespace Weather.Common.Interfaces
{
    public interface ISensorType
    {
        int SensorTypeId { get; set; }
        string Name { get; set; }
    }
}