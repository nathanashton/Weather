namespace Weather.Common.Interfaces
{
    public class IPluginWrapper
    {
        public IPlugin Plugin { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IPluginWrapper(IPlugin plugin, string name, string description)
        {
            Plugin = plugin;
            Name = name;
            Description = description;
        }
    }
}