﻿using PropertyChanged;
using System;
using System.IO;
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

        public string DatabasePath => Path.Combine(ApplicationPath, "weather.sqlite");
        public int RoundingFactor => 1;

        public string ErrorPath { get { return Path.Combine(ApplicationPath, "ErrorReports"); } }
    }
}