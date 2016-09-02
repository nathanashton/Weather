using PropertyChanged;
using System;
using System.IO;
using System.Xml.Serialization;
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
        public string ApplicationPath
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationName);

        [XmlIgnore]
        public string DatabasePath => Path.Combine(ApplicationPath, "weather.sqlite");

        [XmlIgnore]
        public string ErrorPath { get { return Path.Combine(ApplicationPath, "ErrorReports"); } }

        public string Skin { get; set; }

        public void Load()
        {
            if (File.Exists(SettingsFile))
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(Settings));

                using (FileStream myFileStream = new FileStream(SettingsFile, FileMode.Open))
                {
                    var t = (ISettings)mySerializer.Deserialize(myFileStream);
                    this.Skin = t.Skin;
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
            XmlSerializer mySerializer = new XmlSerializer(typeof(Settings));
            using (StreamWriter myWriter = new StreamWriter(SettingsFile))
            {
                 mySerializer.Serialize(myWriter, this);
            myWriter.Close();
            }
           
           
        }
    }
}