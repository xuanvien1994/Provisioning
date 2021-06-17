using System;

namespace Covergo.AADProvisioning.Schema
{
    public abstract class ConfigurationFactory<TConfiguration, TException>
        where TException : Exception
    {
        public abstract TConfiguration Create(
            Lazy<TConfiguration> defaultConfiguration,
            out TException configurationError);
    }
}
