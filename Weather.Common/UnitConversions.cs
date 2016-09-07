namespace Weather.Common
{
    public static class UnitConversions
    {
        public static int Rounding => 1;

        public static double CelsiusToFahrenheit(double celsius)
        {
            var fahrenheit = celsius*1.8000 + 32;
            return fahrenheit;
        }

        public static double CelsiusToKelvin(double celsius)
        {
            var kelvin = celsius + 273.15;
            return kelvin;
        }

        public static double FahrenheitToCelsius(double fahrenheit)
        {
            var celsius = (fahrenheit - 32)/1.8000;
            return celsius;
        }

        public static double FahrenheitToKelvin(double fahrenheit)
        {
            var kelvin = (fahrenheit + 459.67)*5/9;
            return kelvin;
        }

        public static double KelvinToCelsius(double kelvin)
        {
            var celsius = kelvin - 273.15;
            return celsius;
        }

        public static double KelvinToFahrenheit(double kelvin)
        {
            var fahrenheit = kelvin*9/5 - 459.67;
            return fahrenheit;
        }

        public static double HectopascalsToInHg(double hectopascals)
        {
            var inhg = hectopascals*0.029529980164712;
            return inhg;
        }

        public static double HectopascalsToMmHg(double hectopascals)
        {
            return 1;
        }

        public static double MmHgToHectopascals(double inhg)
        {
            return 1;
        }

        public static double InHgToHectopascals(double inhg)
        {
            var hectopascals = inhg/0.029529980164712;
            return hectopascals;
        }

        public static double MmHgToInHg(double inhg)
        {
            return 1;
        }

        public static double InHgToMmHg(double inhg)
        {
            return 1;
        }
    }
}