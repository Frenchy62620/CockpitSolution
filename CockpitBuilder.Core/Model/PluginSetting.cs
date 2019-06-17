using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CockpitBuilder.Core.Model
{
    [DataContract]
    public class PluginSetting
    {
        [DataMember]
        public string PluginType { get; set; }
        [DataMember]
        public List<PluginProperty> PluginProperties { get; set; }

        public PluginSetting(string pluginFamilyName, string pluginType)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(pluginFamilyName);
            PluginType = pluginType;
            PluginProperties = new List<PluginProperty>();
        }
    }
}