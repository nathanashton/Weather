using PropertyChanged;
using Weather.Common;
using Weather.Common.Entities;
using Weather.Common.Units;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class SensorWindowViewModel : NotifyBase
    {
        public SensorWindowViewModel()
        {
            Sensor = new Sensor();
        }

      //  public UnitType SelectedType { get; set; }

        public Sensor Sensor { get; set; }
        public Sensor EditSensor { get; set; }

        public string CorrectionValue { get; set; }
        //    => EnumHelper.GetAllValuesAndDescriptions<UnitType>();

        //public IEnumerable<KeyValuePair<string, string>> Types
    }
}