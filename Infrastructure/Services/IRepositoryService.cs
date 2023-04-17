using System.Linq.Expressions;

namespace Infrastructure.Services
{
    public interface IRepositoryService
    {
        Task<int> CountAsync<T>(Expression<Func<T, bool>> spec) where T : class;
        Task<List<T>> GetAsync<T>() where T : class;
        Task<List<T>> GetAsync<T>(Expression<Func<T, bool>> spec) where T : class;
        Task<List<T>> GetAsync<T>(Expression<Func<T, bool>> match, Expression<Func<T, T>> select) where T : class;
        Task<T> GetOneAsync<T>(Expression<Func<T, bool>> spec) where T : class;
        Task<T> GetOneAsync<T>(Expression<Func<T, bool>> match, Expression<Func<T, T>> select) where T : class;
        Task<T> GetOneRecentAsync<T>(Expression<Func<T, bool>> match, Expression<Func<T, dynamic>> matchDate) where T : class;
        Task<T> GetByIdAsync<T>(Guid id) where T : class;
        Task<T> CreateAsync<T>(T entity) where T : class;
        Task<T> CreateWithNoTrackingAsync<T>(T entity) where T : class;
        Task<T> CreateIfNotExistsAsync<T>(T entity, Guid id) where T : class;
        Task<T> UpdateAsync<T>(T updated, Guid key) where T : class;
        Task<T> UpsertAsync<T>(T entity, Guid key) where T : class;
        Task UpdatePropertyAsync<T>(T updated, params Expression<Func<T, object>>[] properties) where T : class;
        Task UpdatePropertyAsync<T>(T updated, string propety) where T : class;
        Task<int> DeleteAsync<T>(T entity) where T : class;
        Task<T> SoftDeleteAsync<T>(T entity) where T : class;
        Task DeleteIfExistsAsync<T>(T entity, Guid id) where T : class;
        Task DeleteManyAsync<T>(IEnumerable<T> entities) where T : class;
        Task DeleteManyAsync<T>(Expression<Func<T, bool>> spec) where T : class;
        Task<List<T>> SoftDeleteManyAsync<T>(Expression<Func<T, bool>> spec) where T : class;
        Task SoftDeleteManyAsync<T>(IEnumerable<T> entities) where T : class;
    }
}
