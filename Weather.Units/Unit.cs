using Weather.Units.Interfaces;

namespace Weather.Units
{
    public class Unit : IUnit
    {
        public int UnitId { get; set; }
        public UnitType UnitType { get; set; }
        public UnitEnums.EnumUnit EnumUnit { get; set; }
        public string DisplayName { get; set; }
        public string DisplayUnit { get; set; }

        public override string ToString()
        {
            return DisplayUnit;
        }
    }
}