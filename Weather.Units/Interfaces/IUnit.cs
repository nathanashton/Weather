namespace Weather.Units.Interfaces
{
    public interface IUnit
    {
        int UnitId { get; set; }
        UnitEnums.EnumUnitType EnumType { get; set; }
        UnitEnums.EnumUnit EnumUnit { get; set; }
        string DisplayUnit { get; set; }
        string DisplayName { get; set; }
    }
}