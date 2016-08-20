namespace Weather.Common.Units
{
    public class Unit
    {
        public int UnitId { get; set; }
        public UnitType Type { get; set; }
        public string DisplayName { get; set; }
        public string DisplayUnit { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}