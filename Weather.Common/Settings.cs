using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
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
        public string DatabaseConnectionString
            => @"Data Source=..\..\..\Weather.Repository\weather.sqlite;Version=3;foreign keys=false;";

        [XmlIgnore]
        public string ApplicationPath
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationName);

        [XmlIgnore]
        public string DatabasePath => Path.Combine(ApplicationPath, "weather.sqlite");

        [XmlIgnore]
        public string ErrorPath => Path.Combine(ApplicationPath, "ErrorReports");


        public void Load()
        {
            if (File.Exists(SettingsFile))
            {
                var paletteHelper = new PaletteHelper();
                var swatchProvider = new SwatchesProvider();
                var s = swatchProvider.Swatches;
                var mySerializer = new XmlSerializer(typeof(Settings));

                using (var myFileStream = new FileStream(SettingsFile, FileMode.Open))
                {
                    var t = (ISettings) mySerializer.Deserialize(myFileStream);
                    if (!string.IsNullOrEmpty(t.PrimaryColor))
                    {
                        var swatch = swatchProvider.Swatches.FirstOrDefault(x => x.Name == t.PrimaryColor);
                        paletteHelper.ReplacePrimaryColor(swatch);
                    }
                    else
                    {
                        var swatch = swatchProvider.Swatches.FirstOrDefault(x => x.Name == "indigo");
                        paletteHelper.ReplacePrimaryColor(swatch);
                    }

                    if (!string.IsNullOrEmpty(t.AccentColor))
                    {
                        var swatch = swatchProvider.Swatches.FirstOrDefault(x => x.Name == t.AccentColor);
                        paletteHelper.ReplacePrimaryColor(swatch);
                    }
                    else
                    {
                        var swatch = swatchProvider.Swatches.FirstOrDefault(x => x.Name == "yellow");
                        paletteHelper.ReplaceAccentColor(swatch);
                    }
                    paletteHelper.SetLightDark(t.IsDark);
                }
            }
            else
            {
                //Defaults
                PrimaryColor = "indigo";
                AccentColor = "yellow";
                IsDark = true;
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

        public string PrimaryColor { get; set; }
        public string AccentColor { get; set; }
        public bool IsDark { get; set; }
    }
}