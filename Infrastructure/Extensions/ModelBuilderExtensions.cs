using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Converts the enum properties to string.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void ConvertEnumPropertiesToString(this ModelBuilder modelBuilder)
        {
        }

        /// <summary>
        /// Filters the models. E.g. Don't retrieve records from the database that have been soft deleted.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void FilterModels(this ModelBuilder modelBuilder)
        {
            // Filter out all entities that are soft deleted
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var isDeletedProperty = entityType.FindProperty("DateDeleted");
                if (isDeletedProperty != null)
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "x");
                    var body = Expression.Equal(Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(DateTime) }, parameter, Expression.Constant("DateDeleted")), Expression.Constant(false));
                    var filter = Expression.Lambda(body, parameter);
                    entityType.SetQueryFilter(filter);
                }
            }
        }

        /// <summary>
        /// Sets the foreign key behavior.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void SetForeignKeyBehavior(this ModelBuilder modelBuilder)
        {
            // Activation Request
            // modelBuilder.Entity<ENTITY_NAME>().HasOne(ar => ar.ServicePlan).WithMany().OnDelete(DeleteBehavior.NoAction);
        }
    }
}
