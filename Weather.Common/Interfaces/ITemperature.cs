namespace Weather.Common.Interfaces
{
    public interface ITemperature : ISensorValue
    {
        void DisplayDegreesCelsius();

        void DisplayDegreesFahrenheit();

        void DisplayKelvin();
    }
}