using Domain.Devices.Domain;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Sets the foreign key behavior. E.g.: one-to-many relationships
            modelBuilder.SetForeignKeyBehavior();

            // Filter records that have been soft deleted
            modelBuilder.FilterModels();

            // Convert enums to their string value
            modelBuilder.ConvertEnumPropertiesToString();

            // Configures the schemas
            base.OnModelCreating(modelBuilder);
        }

        // Keep these db sets alphabetical 
        public DbSet<IoTData> IoTData { get; set; }
    }
}