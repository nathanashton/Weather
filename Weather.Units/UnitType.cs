using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Units.Interfaces;

namespace Weather.Units
{
    public class UnitType : IUnitType
    {
        public int UnitTypeId { get; set; }
        public UnitEnums.EnumUnitType Type { get; set; }
        public string DisplayName { get; set; }
    }
}
