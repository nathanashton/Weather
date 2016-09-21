using Weather.Charts.WindPolar;
using Weather.Common;
using Weather.Common.Interfaces;

namespace Weather.Charts
{
    [DisplayName("Wind (Polar)")]
    [Description("Radial polar chart of wind speed and direction.")]
    public class WindPolarPlugin : IPlugin
    {
        public WindPolarPlugin(ISelectedStation selectedStation)
        {
            SelectedStation = selectedStation;
        }

        public ISelectedStation SelectedStation { get; set; }

        public IChartCodeBehind View => new WindPolarControl(SelectedStation);

        public IChartViewModel ViewModel => View.ViewModel;
    }
}