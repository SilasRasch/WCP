using System.Linq.Expressions;

namespace WCPShared.Interfaces.DataServices
{
    public interface IDatabaseServiceExtensions <T> where T : class
    {
        /// <summary>
        /// Get all objects fulfulling a criteria (predicate)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>A list of entities or an empty list</returns>
        Task<List<T>> GetObjectsBy(Expression<Func<T, bool>> predicate);
        
        /// <summary>
        /// Get a single object fulfulling a criteria (predicate)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T?> GetObjectBy(Expression<Func<T, bool>> predicate);
    }
}
