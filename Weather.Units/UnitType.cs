using System.Collections.Generic;
using Weather.Units.Interfaces;

namespace Weather.Units
{
    public class UnitType : IUnitType
    {
        public int UnitTypeId { get; set; }
        public UnitEnums.EnumUnitType Type { get; set; }
        public string DisplayName { get; set; }
        public List<IUnit> Units { get; set; }
    }
}