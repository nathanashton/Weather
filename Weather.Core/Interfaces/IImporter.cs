using Weather.Common.Entities;

namespace Weather.Core.Interfaces
{
    public interface IImporter
    {
        void Import(string filePath, WeatherStation station);
    }
}