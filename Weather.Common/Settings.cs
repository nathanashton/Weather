using System;
using System.IO;
using System.Xml.Serialization;
using PropertyChanged;
using Weather.Common.Interfaces;

namespace Weather.Common
{
    [ImplementPropertyChanged]
    [Serializable]
    public class Settings : ISettings
    {
        public Settings()
        {
        }

        public Settings(ILog log)
        {
            log.SetInfoLevel();
        }

        public string ApplicationName => "Weather";

        [XmlIgnore]
        public string SettingsFile => Path.Combine(ApplicationPath, "settings.xml");

        [XmlIgnore]
        public string DatabaseConnectionString => @"Data Source=..\..\..\Weather.Repository\weather.sqlite;Version=3;";

        [XmlIgnore]
        public string ApplicationPath
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationName);

        [XmlIgnore]
        public string DatabasePath => Path.Combine(ApplicationPath, "weather.sqlite");

        [XmlIgnore]
        public string ErrorPath => Path.Combine(ApplicationPath, "ErrorReports");

        public string Skin { get; set; }

        public void Load()
        {
            if (File.Exists(SettingsFile))
            {
                var mySerializer = new XmlSerializer(typeof(Settings));

                using (var myFileStream = new FileStream(SettingsFile, FileMode.Open))
                {
                    var t = (ISettings) mySerializer.Deserialize(myFileStream);
                    Skin = t.Skin;
                }
            }
            else
            {
                //Defaults
                Skin = "Dark";
                Save();
            }
        }

        public void Save()
        {
            var mySerializer = new XmlSerializer(typeof(Settings));
            using (var myWriter = new StreamWriter(SettingsFile))
            {
                mySerializer.Serialize(myWriter, this);
                myWriter.Close();
            }
        }
    }
}