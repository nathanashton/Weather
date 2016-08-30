using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Common.Interfaces;

namespace Weather.Repository.Interfaces
{
    public interface IStationSensorRepository
    {
        IStationSensor GetById(int id);
        void Update(IStationSensor stationSensor);
    }
}
