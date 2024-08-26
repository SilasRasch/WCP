using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCPShared.Interfaces
{
    public interface IDatabaseService<T> where T : class
    {
        Task AddObject(T obj);
        Task<IEnumerable<T>> GetAllObjects();
        Task<T?> GetObject(int id);
        Task<T?> UpdateObject(int id, T obj);
        Task<T?> DeleteObject(int id);
    }
}
