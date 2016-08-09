using PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using Weather.Common.Entities;
using Weather.Core.Interfaces;

namespace Weather.Core
{
    [ImplementPropertyChanged]
    public class StationCore : IStationCore
    {
        public StationCore()
        {
            GetAllStations();
        }

        public ObservableCollection<WeatherStation> Stations { get; set; }

        public void DeleteStation(WeatherStation station)
        {
            using (var ctx = new Database())
            {
                ctx.WeatherStations.Attach(station);
                ctx.WeatherStations.Remove(station);
                ctx.SaveChanges();
                Stations.Remove(station);
            }
        }

        public void UpdateStation(WeatherStation station)
        {
            using (var ctx = new Database())
            {
                var existing = ctx.WeatherStations.FirstOrDefault(x => x.Id == station.Id);
                if (existing == null) return;
                ctx.Entry(existing).CurrentValues.SetValues(station);
                ctx.Entry(existing).State = EntityState.Modified;
                ctx.SaveChanges();
                Stations.Remove(station);
                Stations.Add(station);
            }
        }

        public void AddSensor(Sensor sensor)
        {
            using (var ctx = new Database())
            {
                ctx.Sensors.Attach(sensor);
                ctx.Entry(sensor).State = EntityState.Added;
                ctx.SaveChanges();
            }
        }

        public void AddWeatherRecord(WeatherRecord record)
        {
            using (var ctx = new Database())
            {
                ctx.WeatherRecords.Attach(record);
                ctx.Entry(record).State = EntityState.Added;
               // ctx.WeatherRecords.Add(record);
                ctx.SaveChanges();
            }
        }

        public void AddWeatherRecords(IEnumerable<WeatherRecord> records)
        {
            using (var ctx = new Database())
            {
                ctx.WeatherRecords.AddRange(records);
                ctx.SaveChanges();
                GetAllStations();
            }
        }

        public void DeleteSensor(Sensor sensor)
        {
            using (var ctx = new Database())
            {
                var foundsensorvalues = ctx.SensorValues.Where(x => x.Sensor.Id == sensor.Id);
                ctx.SensorValues.RemoveRange(foundsensorvalues);
                ctx.SaveChanges();
                var found = ctx.Sensors.Find(sensor.Id);

                ctx.Entry(found).State = EntityState.Deleted;
                ctx.SaveChanges();
            }

            //TODO if all sensors for a weatherrecord are deleted the weather record still exists. It should be removed too.
        }

        public void UpdateSensor(Sensor sensor)
        {
            using (var ctx = new Database())
            {
                var existing = ctx.Sensors.FirstOrDefault(x => x.Id == sensor.Id);
                if (existing == null) return;
                ctx.Entry(existing).CurrentValues.SetValues(sensor);
                ctx.Entry(existing).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public WeatherStation SelectedStation { get; set; }

        public void AddStation(WeatherStation station)
        {
            using (var ctx = new Database())
            {
                ctx.WeatherStations.Add(station);
                ctx.SaveChanges();
                Stations.Add(station);
            }
        }

        public List<WeatherStation> GetAllStations()
        {
            using (var ctx = new Database())
            {
                if (Stations == null)
                {
                    Stations = new ObservableCollection<WeatherStation>();
                }
                Stations.Clear();
                var all = ctx.WeatherStations.Include(x => x.WeatherRecords.Select(p=> p.SensorValues)).Include(y => y.Sensors).ToList();

                return all;
            }
        }
    }
}