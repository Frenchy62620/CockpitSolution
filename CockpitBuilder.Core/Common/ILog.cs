using System;

namespace CockpitBuilder.Core.Common
{
    public interface ILog
    {
        void Error(Exception e);
    }
}