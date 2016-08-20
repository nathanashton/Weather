﻿using System.Collections.Generic;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Common.Units;

namespace Weather.Core.Interfaces
{
    public interface ISensorTypeCore
    {
        List<ISensorType> GetAll();
        ISensorType GetById(int id);
        ISensorType Add(ISensorType sensorType);
        void Delete(ISensorType sensorType);
        void Update(ISensorType sensorType);
        ISensorType AddOrUpdate(ISensorType sensorType);
        ISensorType AddUnitToSensorType(Unit unit, ISensorType sensorType);
    }
}