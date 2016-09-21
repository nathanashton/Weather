using Weather.Charts.LineGraph;
using Weather.Common;
using Weather.Common.Interfaces;

namespace Weather.Charts
{
    [DisplayName("Line Graph / 2 Sensors")]
    [Description("Graph One or Two sensors at the same time.")]
    public class LineGraphPlugin : IPlugin
    {
        public LineGraphPlugin(ISelectedStation selectedStation)
        {
            SelectedStation = selectedStation;
        }

        public ISelectedStation SelectedStation { get; set; }

        public IChartCodeBehind View => new LineGraphControl(SelectedStation);

        public IChartViewModel ViewModel => View.ViewModel;
    }
}