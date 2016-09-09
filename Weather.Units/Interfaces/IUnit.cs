namespace Weather.Units.Interfaces
{
    public interface IUnit
    {
        int UnitId { get; set; }
        UnitType UnitType { get; set; }
        UnitEnums.EnumUnit EnumUnit { get; set; }
        string DisplayUnit { get; set; }
        string DisplayName { get; set; }
    }
}