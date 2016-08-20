using System.Collections.Generic;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Common.Units;

namespace Weather.Core.Interfaces
{
    public interface IUnitCore
    {
        List<Unit> GetAll();
        Unit GetById(int id);
        Unit Add(Unit unit);
        void Delete(Unit unit);
        void Update(Unit unit);
        Unit AddOrUpdate(Unit unit);
    }
}