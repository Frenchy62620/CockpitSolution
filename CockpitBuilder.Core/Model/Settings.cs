using System.Collections.Generic;
using System.Linq;

namespace CockpitBuilder.Core.Model
{
    public class Settings
    {
        //public List<Curve> Curves { get; set; }
        public List<PluginSetting> PluginSettings { get; set; }
        //public bool MinimizeToTray { get; set; }
        public string Basedirectory { get; set; }
        public List<string> RecentProfiles { get; set; }

        public Settings()
        {
            PluginSettings = new List<PluginSetting>();
            //Curves = new List<Curve>();
            RecentProfiles = new List<string>();
        }

        public void AddPluginSetting(PluginSetting pluginSetting)
        {
            PluginSettings.Add(pluginSetting);
        }

        //public void AddNewCurve(Curve curve)
        //{
        //    Curves.Add(curve);
        //    Curves = new List<Curve>(Curves.OrderBy(c => c.Name));
        //}

        //public void RemoveCurve(Curve curve)
        //{
        //    Curves.Remove(curve);
        //}

        public void AddRecentProfile(string path)
        {
            if (path != null)
            {
                const int n = 10;
                RecentProfiles.Remove(path);
                RecentProfiles.Insert(0, path);
                if (RecentProfiles.Count > n)
                    RecentProfiles.RemoveRange(n, RecentProfiles.Count - n);
            }
        }
    }
}