using System;

namespace CockpitBuilder.Core.Persistence.Paths
{
    public class PortablePaths : Paths
    {
        public PortablePaths()
        {
            Application = AppDomain.CurrentDomain.BaseDirectory;
            Data = Application;

            EnureWorkingDirectory();
        }
    }
}
