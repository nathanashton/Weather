namespace Weather.Common.Interfaces
{
    public interface ISettings
    {
        string ApplicationName { get; }
        string ApplicationPath { get; }
        string ErrorPath { get; }
        string DatabasePath { get; }

        string PrimaryColor { get; set; }
        string AccentColor { get; set; }
        bool IsDark { get; set; }

        string SettingsFile { get; }
        string DatabaseConnectionString { get; }

        void Save();
        void Load();
    }
}