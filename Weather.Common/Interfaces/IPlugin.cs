using Weather.Interfaces;

namespace Weather.Common.Interfaces
{
    public interface IPlugin
    {
        ISelectedStation SelectedStation { get; set; }
        IChartCodeBehind View { get; }
        IChartViewModel ViewModel { get; }
    }
}