using System.Data.Entity;
using Weather.Common.Entities;
using Weather.Core.Migrations;

namespace Weather.Core
{
    public class Database : DbContext
    {
        public Database() : base("Data Source=weather.sdf")
        {
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<Database, Configuration>());
        }

        public DbSet<WeatherStation> WeatherStations { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<WeatherRecord> WeatherRecords { get; set; }
        public DbSet<SensorValue> SensorValues { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sensor>().HasKey(x => x.Id);
            modelBuilder.Entity<Sensor>().HasRequired(x => x.Station);

            modelBuilder.Entity<SensorValue>().HasKey(x => x.Id);
            modelBuilder.Entity<SensorValue>().HasRequired(x => x.Sensor).WithMany(y=> y.SensorValues).WillCascadeOnDelete();

            modelBuilder.Entity<WeatherStation>().HasKey(x => x.Id);
            modelBuilder.Entity<WeatherStation>().HasMany(x => x.Sensors).WithRequired(x=> x.Station).WillCascadeOnDelete();
            modelBuilder.Entity<WeatherStation>().HasMany(x => x.WeatherRecords).WithRequired(x => x.Station).WillCascadeOnDelete();

            modelBuilder.Entity<WeatherRecord>().HasKey(x => x.Id);
            modelBuilder.Entity<WeatherRecord>().HasRequired(x => x.Station);
            modelBuilder.Entity<WeatherRecord>()
                .HasMany(x => x.SensorValues);

        }

        public void ClearAll()
        {
            WeatherRecords.RemoveRange(WeatherRecords);
            Sensors.RemoveRange(Sensors);
            SensorValues.RemoveRange(SensorValues);
        }
    }
}