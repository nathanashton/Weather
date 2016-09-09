using System.Collections.Generic;
using System.Linq;
using Weather.Units.Interfaces;

namespace Weather.Units
{
    public class UnitTypes
    {
        public static List<IUnitType> AllUnitTypes { get; set; }

        public UnitTypes()
        {
            AllUnitTypes = new List<IUnitType>
            {
                new UnitType
                {
                    UnitTypeId = 1,
                    Type = UnitEnums.EnumUnitType.Pressure,
                    DisplayName = "Pressure"
                }
            };
        }

        public static IUnitType GetUnitType(UnitEnums.EnumUnitType type)
        {
            return AllUnitTypes.FirstOrDefault(x => x.Type == type);
        }

        public static IUnitType GetUnitTypeById(int id)
        {
            return AllUnitTypes.FirstOrDefault(x => x.UnitTypeId == id);
        }
    
    }
}