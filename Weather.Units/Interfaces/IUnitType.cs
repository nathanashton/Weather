namespace Weather.Units.Interfaces
{
    public interface IUnitType
    {
        int UnitTypeId { get; set; }
        UnitEnums.EnumUnitType Type { get; set; }
        string DisplayName { get; set; }
    }
}