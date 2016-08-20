using System.Collections.ObjectModel;
using Weather.Common.Units;

namespace Weather.Common.Interfaces
{
    public interface ISensorType
    {
        int SensorTypeId { get; set; }
        string Name { get; set; }
        ObservableCollection<Unit> Units { get; set; }
    }
}