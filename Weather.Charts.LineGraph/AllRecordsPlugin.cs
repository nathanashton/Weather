using Weather.Common;
using Weather.Common.Interfaces;
using Weather.Interfaces;

namespace Weather.Charts
{
    [DisplayName("All Records")]
    [Description("Grid of all records.")]
    public class AllRecordsPlugin : IPlugin
    {
        public AllRecordsPlugin(ISelectedStation selectedStation)
        {
            SelectedStation = selectedStation;
        }

        public ISelectedStation SelectedStation { get; set; }

        public IChartCodeBehind View => new AllRecords.AllRecords(SelectedStation);

        public IChartViewModel ViewModel => View.ViewModel;
    }
}