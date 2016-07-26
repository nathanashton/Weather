namespace Weather.Common.Interfaces
{
    public interface ISettings
    {
        string ApplicationName { get; }
        string ApplicationPath { get; }
        string DatabasePath { get; }
        int RoundingFactor { get; }
    }
}