using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly ApplicationDbContext _context;

        public RepositoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the asynchronously.
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> CountAsync<T>(Expression<Func<T, bool>> match) where T : class
        {
            var result = await _context.Set<T>().Where(match).ToListAsync();
            return result.Count();
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<T>> GetAsync<T>(Expression<Func<T, bool>> match) where T : class
        {
            return await _context.Set<T>().Where(match).ToListAsync();
        }

        /// <summary>
        /// Gets the entities asynchronously.
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<T>> GetAsync<T>() where T : class
        {
            return await _context.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Gets the specific asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public async Task<List<T>> GetAsync<T>(Expression<Func<T, bool>> match, Expression<Func<T, T>> select) where T : class
        {
            return await _context.Set<T>().Where(match).Select(select).ToListAsync();
        }

        /// <summary>
        /// Gets the one entity asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public async Task<T> GetOneAsync<T>(Expression<Func<T, bool>> match) where T : class
        {
            return await _context.Set<T>().SingleOrDefaultAsync(match);
        }

        /// <summary>
        /// Gets the one async2.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="match">The match.</param>
        /// <param name="select">The select.</param>
        /// <returns></returns>
        public async Task<T> GetOneAsync<T>(Expression<Func<T, bool>> match, Expression<Func<T, T>> select) where T : class
        {
            return await _context.Set<T>().Where(match).Select(select).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Gets the one recent asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="match">The match.</param>
        /// <param name="dateTimeLabel">The date time label.</param>
        /// <returns></returns>
        public async Task<T> GetOneRecentAsync<T>(Expression<Func<T, bool>> match, Expression<Func<T, dynamic>> matchDate) where T : class
        {
            var query = await _context.Set<T>().OrderByDescending(matchDate).Where(match).ToListAsync(new CancellationToken());
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<T> GetByIdAsync<T>(Guid id) where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<T> CreateAsync<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return entity;
        }

        /// <summary>
        /// Creates the entity with no tracking asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<T> CreateWithNoTrackingAsync<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
            try
            {
                await _context.SaveChangesAsync();
                _context.Entry(entity).State = EntityState.Detached;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return entity;
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<T> CreateIfNotExistsAsync<T>(T entity, Guid id) where T : class
        {
            var exists = await _context.Set<T>().FindAsync(id);

            if (exists != null)
                return exists;

            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Upserts the entity asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<T> UpsertAsync<T>(T entity, Guid key) where T : class
        {
            if (entity == null)
                return null;

            T existing = await _context.Set<T>().FindAsync(key);

            if (existing == null)
            {
                _context.Set<T>().Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            return existing;
        }

        /// <summary>
        /// Updates the entity asynchronously.
        /// </summary>
        /// <param name="updated">The updated.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<T> UpdateAsync<T>(T updated, Guid key) where T : class
        {
            if (updated == null)
                return null;

            T existing = await _context.Set<T>().FindAsync(key);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(updated);
                await _context.SaveChangesAsync();
            }

            return existing;
        }

        /// <summary>
        /// Updates the property asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updated">The updated.</param>
        /// <param name="propertyName">DayOfWeek of the property.</param>
        /// <returns></returns>
        public async Task UpdatePropertyAsync<T>(T entity, params Expression<Func<T, object>>[] properties) where T : class
        {
            _context.Set<T>().Attach(entity);
            foreach (var p in properties)
                _context.Entry(entity).Property(p).IsModified = true;

            await _context.SaveChangesAsync();
            return;
        }

        /// <summary>
        /// Updates the property asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updated">The updated.</param>
        /// <param name="propety">The propety.</param>
        public async Task UpdatePropertyAsync<T>(T updated, string propety) where T : class
        {
            _context.Attach(updated).Property(propety);
            await _context.SaveChangesAsync();
            return;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> DeleteAsync<T>(T entity) where T : class
        {
            _context.Set<T>().Attach(entity);
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Softs the delete asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        public async Task<T> SoftDeleteAsync<T>(T entity) where T : class
        {
            _context.Attach(entity).Property("DateTimeDeleted").CurrentValue = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            _context.Attach(entity).Property("IsDeleted").CurrentValue = true;
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Deletes if exists asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task DeleteIfExistsAsync<T>(T entity, Guid id) where T : class
        {
            var exists = await _context.Set<T>().FindAsync(id);

            if (exists == null)
                return;

            _context.Set<T>().Attach(entity);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return;
        }

        /// <summary>
        /// Deletes the many asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">The entities.</param>
        public async Task DeleteManyAsync<T>(IEnumerable<T> entities) where T : class
        {
            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync();
            return;
        }

        /// <summary>
        /// Deletes the many asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spec">The spec.</param>
        public async Task DeleteManyAsync<T>(Expression<Func<T, bool>> spec) where T : class
        {
            var entities = await _context.Set<T>().Where(spec).ToListAsync();
            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync();
            return;
        }

        /// <summary>
        /// Softs the delete many asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spec">The spec.</param>
        public async Task<List<T>> SoftDeleteManyAsync<T>(Expression<Func<T, bool>> spec) where T : class
        {
            var entities = await _context.Set<T>().Where(spec).ToListAsync();
            foreach (var entity in entities)
            {
                _context.Attach(entity).Property("DateTimeDeleted").CurrentValue = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                _context.Attach(entity).Property("IsDeleted").CurrentValue = true;
                await _context.SaveChangesAsync();
            }
            return entities;
        }

        /// <summary>
        /// Soft deletes many asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">The entities.</param>
        public async Task SoftDeleteManyAsync<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                _context.Attach(entity).Property("DateTimeDeleted").CurrentValue = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                _context.Attach(entity).Property("IsDeleted").CurrentValue = true;
                await _context.SaveChangesAsync();
            }
            return;
        }
    }
}
