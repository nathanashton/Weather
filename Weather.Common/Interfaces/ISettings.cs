namespace Weather.Common.Interfaces
{
    public interface ISettings
    {
        string ApplicationName { get; }
        string ApplicationPath { get; }
        string ErrorPath { get; }
        string DatabasePath { get; }
        string Skin { get; set; }
        string SettingsFile { get; }

        void Save();
        void Load();
    }
}