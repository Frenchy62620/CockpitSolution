using CockpitBuilder.Core.Model;
using System.Collections.Generic;
//using FreePIE.Core.Contracts;

namespace CockpitBuilder.Core.Persistence
{
    public interface ISettingsManager
    {
        bool Load();
        void Save();
        Settings Settings { get; }
        //PluginSetting GetPluginSettings(IPlugin plugin);
        //IEnumerable<PluginSetting> ListConfigurablePluginSettings();
        //IEnumerable<PluginSetting> ListPluginSettingsWithHelpFile();
    }
}