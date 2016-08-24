using System.Collections.Generic;
using Weather.Common.Units;

namespace Weather.Repository.Interfaces
{
    public interface IUnitRepository
    {
        List<Unit> GetAll();

        Unit GetById(int id);

        int Add(Unit unit);

        void Delete(int id);

        void Update(Unit unit);


    }
}