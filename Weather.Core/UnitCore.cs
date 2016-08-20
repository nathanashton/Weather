using System.Collections.Generic;
using Weather.Common.Interfaces;
using Weather.Common.Units;
using Weather.Core.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Core
{
    public class UnitCore : IUnitCore
    {
        private readonly IUnitRepository _repository;

        public UnitCore(IUnitRepository repository)
        {
            _repository = repository;
        }

        public List<Unit> GetAll()
        {
            return _repository.GetAll();
        }

        public Unit GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Unit Add(Unit unit)
        {
            unit.UnitId = _repository.Add(unit);
            return unit;
        }

        public void Delete(Unit unit)
        {
            _repository.Delete(unit.UnitId);
        }

        public void Update(Unit unit)
        {
            _repository.Update(unit);
        }

        public Unit AddOrUpdate(Unit unit)
        {
            if (unit.UnitId == 0)
            {
                unit.UnitId = _repository.Add(unit);
                return unit;
            }
            Update(unit);
            return unit;
        }
    }
}