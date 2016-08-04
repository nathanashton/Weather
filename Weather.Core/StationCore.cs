using System.Collections.Generic;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;
using Weather.Common.Entities;
using Weather.Core.Interfaces;

namespace Weather.Core
{
    [ImplementPropertyChanged]
    public class StationCore : IStationCore
    {
        public ObservableCollection<WeatherStation> Stations { get; set; }
        private readonly Database _context;

        public void DeleteStation(WeatherStation station)
        {
            _context.WeatherStations.Remove(station);
            _context.SaveChanges();
            Stations.Remove(station);
        }

        public void UpdateStation(WeatherStation station)
        {
            var existing = _context.WeatherStations.FirstOrDefault(x => x.Id == station.Id);
            if (existing == null) return;
            _context.Entry(existing).CurrentValues.SetValues(station);
            _context.Entry(existing).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
            Stations.Remove(station);
            Stations.Add(station);
        }

        public void AddSensor(Sensor sensor)
        {
            _context.Sensors.Add(sensor);
            _context.SaveChanges();
        }

        public void AddWeatherRecord(WeatherRecord record)
        {
            _context.WeatherRecords.Add(record);
            _context.SaveChanges();
        }

        public void AddWeatherRecords(IEnumerable<WeatherRecord> records)
        {
            _context.WeatherRecords.AddRange(records);
            _context.SaveChanges();
            GetAllStations();
        }

        public StationCore(Database ctx)
        {
            _context = ctx;
            GetAllStations();
        }

        public void DeleteSensor(Sensor sensor)
        {
            var foundsensorvalues = _context.SensorValues.Where(x => x.Sensor.Id == sensor.Id);
            _context.SensorValues.RemoveRange(foundsensorvalues);
            _context.SaveChanges();
            var found = _context.Sensors.Find(sensor.Id);

            _context.Entry(found).State = System.Data.Entity.EntityState.Deleted;
            _context.SaveChanges();

            //TODO if all sensors for a weatherrecord are deleted the weather record still exists. It should be removed too.
        }

        public void UpdateSensor(Sensor sensor)
        {
            var existing = _context.Sensors.FirstOrDefault(x => x.Id == sensor.Id);
            if (existing == null) return;
            _context.Entry(existing).CurrentValues.SetValues(sensor);
            _context.Entry(existing).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void AddStation(WeatherStation station)
        {
            _context.WeatherStations.Add(station);
            _context.SaveChanges();
            Stations.Add(station);
        }

        public void GetAllStations()
        {
            if (Stations == null)
            {
                Stations = new ObservableCollection<WeatherStation>();
            }
            Stations.Clear();
            var all = _context.WeatherStations.ToList();
            foreach (var station in all)
            {
                Stations.Add(station);
            }
        }
    }
}