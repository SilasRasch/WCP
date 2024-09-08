using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.Views;
using WCPShared.Models;

namespace WCPShared.Interfaces.DataServices.Utility
{
    /// <summary>
    /// An interface for fetching safe view models of a unsafe class
    /// </summary>
    /// <typeparam name="T">Input</typeparam>
    /// <typeparam name="X">Output</typeparam>
    public interface IObjectViewService <T, X> where T : class where X : class
    {
        Task<List<X>> GetObjectsViewBy(Expression<Func<T, bool>> predicate);
        Task<X?> GetObjectViewBy(Expression<Func<T, bool>> predicate);
        Task<List<X>> GetAllObjectsView();
    }
}
