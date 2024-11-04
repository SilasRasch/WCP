using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCPShared.Interfaces.DataServices
{
    /// <summary>
    /// Interface that defines the use of DTOs in a service class
    /// </summary>
    /// <typeparam name="T">Input (DTO)</typeparam>
    /// <typeparam name="X">Output</typeparam>
    public interface IDtoExtensions<T, X> where T : class where X : class
    {
        Task<X?> AddObject(T obj);
        Task<X?> UpdateObject(int id, T obj);
    }
}
