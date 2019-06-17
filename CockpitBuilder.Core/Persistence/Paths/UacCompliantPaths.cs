using System;
using System.IO;
using CockpitBuilder.Core.Common;

namespace CockpitBuilder.Core.Persistence.Paths
{
    public class UacCompliantPaths : Paths
    {
        private const string appFolder = "%appdata%\\HeliosCaliburn";

        public UacCompliantPaths(IFileSystem fileSystem)
        {
            var absoluteDataPath = Environment.ExpandEnvironmentVariables(appFolder);

            fileSystem.CreateDirectory(absoluteDataPath);

            Data = absoluteDataPath;
            Application = AppDomain.CurrentDomain.BaseDirectory;
            EnureWorkingDirectory();
        }
    }
}
