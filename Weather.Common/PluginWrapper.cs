using Weather.Common.Interfaces;

namespace Weather.Common
{
    public class PluginWrapper : IPluginWrapper
    {
        public PluginWrapper(IPlugin plugin, string name, string description) : base(plugin, name, description)
        {
        }
    }
}