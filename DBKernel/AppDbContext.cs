using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskWithYou.Shared.Model;

namespace DBKernel
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _Configuration;

        public DbSet<TaskTicket> TaskTickets { get; set; }
        public DbSet<TaskState> TaskStates { get; set; }
        public DbSet<Cluster> Clusters { get; set; }
        public DbSet<TicketCard> TicketCards { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 
            _Configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskTicket>(a =>
            {
                a.HasKey(b => b.Gid);
                a.Property(b => b.TourokuBi).IsRequired();
                a.Property(b => b.Name).IsRequired();
                a.Property(b => b.KigenBi);
                a.Property(b => b.Detail);
                a.Property(b => b.StateGid).IsRequired();
                a.Ignore(b => b.State);
                a.Property(b => b.IsTodayTask).IsRequired();
                a.Property(b => b.ClusterGid);
                a.Ignore(b => b.Cluster);
                a.Property(b => b.CardGid).IsRequired();
                a.Ignore(b => b.Card);
            });

            modelBuilder.Entity<TaskState>(a =>
            {
                a.HasKey(b => b.Gid);               
                a.Property(b => b.Name).IsRequired();
                a.Property(b => b.State).IsRequired();
            });

            modelBuilder.Entity<Cluster>(a =>
            {
                a.HasKey(b => b.Gid);
                a.Property(b => b.Name).IsRequired();
                a.Property(b => b.Detail);
            });

            modelBuilder.Entity<TicketCard>(a =>
            {
                a.HasKey(b => b.Gid);
                a.Property(b => b.XCoordinate).IsRequired();
                a.Property(b => b.YCoordinate).IsRequired();
                a.Property(b => b.TaskTicket).IsRequired();
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _Configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlite($"Data Source={connectionString}");
        }
    }
}
