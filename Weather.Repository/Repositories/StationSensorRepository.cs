using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Common.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Repository.Repositories
{
    public class StationSensorRepository : IStationSensorRepository
    {
        public IStationSensor GetById(int id)
        {
            return null;
        }
    }
}
