using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SensorTagMvvm.Domain;

namespace EntMob6UWP.Domain
{
    class DBContext : DbContext
    {
        public DbSet<Humidity> humidities { get; set; }
        public DbSet<Brightness> brightnesses { get; set; }
        public DbSet<Temperature> temperatures { get; set; }
        public DbSet<AirPressure> airPressures { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=db.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Humidity>()
                .HasKey(b => b.ID);
            modelBuilder.Entity<Humidity>()
                .Property(b => b.Percentage).HasColumnName("percentage");

            modelBuilder.Entity<Brightness>()
               .HasKey(b => b.ID);
            modelBuilder.Entity<Brightness>()
                .Property(b => b.Value).HasColumnName("value");

            modelBuilder.Entity<Temperature>()
               .HasKey(b => b.ID);
            modelBuilder.Entity<Temperature>()
                .Property(b => b.Value).HasColumnName("value");

            modelBuilder.Entity<AirPressure>()
               .HasKey(b => b.ID);
            modelBuilder.Entity<AirPressure>()
                .Property(b => b.Value).HasColumnName("value");

        }
    }
}
