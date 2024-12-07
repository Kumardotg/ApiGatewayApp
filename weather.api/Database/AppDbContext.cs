using Microsoft.EntityFrameworkCore;
namespace weather.api
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        public virtual DbSet<WeatherForecast> WeatherForecast { get; set; }

    }
}

