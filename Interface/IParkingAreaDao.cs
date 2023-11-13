using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public interface IParkingAreaDao<T>: IDaoAsync<T>
    {
        public Task<T> GetTotalAndFreeAmountOfSpace(int id);
    }
}
