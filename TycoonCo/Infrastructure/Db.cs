using Microsoft.EntityFrameworkCore;
using TycoonCo.Domain;

namespace TycoonCo.Infrastructure
{
    public class Db : DbContext
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Worker> Workers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Activity>().OwnsMany(a => a.WorkerActivities);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}