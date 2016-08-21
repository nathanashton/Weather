using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Weather.Common.Interfaces;
using Weather.Common.Units;

namespace Weather.Common.Entities
{
    public class SensorType : ISensorType
    {
        public int SensorTypeId { get; set; }
        public string Name { get; set; }
        public List<Unit> Units { get; set; } = new List<Unit>();
    }
}
