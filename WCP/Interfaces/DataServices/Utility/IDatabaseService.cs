using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCPShared.Interfaces.DataServices.Utility
{
    public interface IDatabaseService<T> where T : class
    {
        Task<T> AddObject(T obj);
        Task<List<T>> GetAllObjects();
        Task<T?> GetObject(int id);
        Task<T?> UpdateObject(int id, T obj);
        Task<T?> DeleteObject(int id);
    }
}
