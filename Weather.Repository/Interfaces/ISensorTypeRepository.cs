using System.Collections.Generic;
using Weather.Common.Interfaces;
using Weather.Common.Units;

namespace Weather.Repository.Interfaces
{
    public interface ISensorTypeRepository
    {
        List<ISensorType> GetAll();

        List<ISensorType> GetAllTest();

        ISensorType GetById(int id);

        int Add(ISensorType sensorType);

        void Delete(int id);

        void Update(ISensorType sensorType);

        void AddUnitToSensorType(Unit unit, ISensorType sensorType);

        void RemoveUnitFromSensorType(Unit unit, ISensorType sensortype);

        bool AnySensorTypesUseUnit(Unit unit);
    }
}