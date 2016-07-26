using System;
using System.IO;
using PropertyChanged;
using Weather.Common.Interfaces;

namespace Weather.Common
{
    [ImplementPropertyChanged]
    public class Settings : ISettings
    {
        public Settings(ILog log)
        {
            log.SetInfoLevel();
        }

        public string ApplicationName => "Weather";

        public string ApplicationPath
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationName);

        public string DatabasePath => Path.Combine(ApplicationPath, "database.sdf");
        public int RoundingFactor => 1;
    }
}