using PropertyChanged;
using System.Collections.Generic;
using Weather.Common;
using Weather.Common.Entities;
using Weather.Helpers;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class SensorWindowViewModel : NotifyBase
    {
        public SensorWindowViewModel()
        {
            Sensor = new Sensor();
        }

        public Enums.UnitType SelectedType { get; set; }

        public Sensor Sensor { get; set; }
        public Sensor EditSensor { get; set; }

        public string CorrectionValue { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Types
            => EnumHelper.GetAllValuesAndDescriptions<Enums.UnitType>();
    }
}