using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public interface IParkingSpaceDao<T> : IDaoAsync<T>
    {
        public Task<IEnumerable<T>> GetAllParkingSpacesForParkingAreaAsync(int id);
        public Task<bool> updateStatusAsync(bool status, int id);
    }
}
