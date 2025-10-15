using System;
using System.Threading;
using System.Threading.Tasks;
using cs_api_rental_car_mvc.Entities;
using Microsoft.EntityFrameworkCore;

namespace cs_api_rental_car_mvc.Data
{
    public class RentalCarDbContext : DbContext
    {

        private void SetAuditFields()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified; // soft delete
                        entry.Entity.DeletedAt = DateTimeOffset.UtcNow;
                        break;
                }
            }
        }

        public override int SaveChanges()
        {
            SetAuditFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public RentalCarDbContext(DbContextOptions<RentalCarDbContext> options)
                : base(options)
        {
        }

        public DbSet<CarEntity> Cars => Set<CarEntity>();
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<RentEntity> Rents => Set<RentEntity>();
    }
}