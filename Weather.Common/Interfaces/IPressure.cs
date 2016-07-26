namespace Weather.Common.Interfaces
{
    public interface IPressure : ISensorValue
    {
        void DisplayHectopascals();

        void DisplayInHg();
    }
}