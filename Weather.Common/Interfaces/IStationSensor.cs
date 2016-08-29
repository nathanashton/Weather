using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Common.Interfaces
{
    public interface IStationSensor
    {int StationSensorId { get; set; }
        ISensor Sensor { get; set; }
        double? Correction { get; set; }
        string Notes { get; set; }
    }
}
