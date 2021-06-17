using System;
using System.Collections.Generic;
using Microsoft.SystemForCrossDomainIdentityManagement;

namespace Covergo.AADProvisioning.Provider
{
    public class InMemoryStorage
    {
        internal readonly IDictionary<string, Core2Group> Groups;
        internal readonly IDictionary<string, Schema.Core2EnterpriseUser> Users;

        private InMemoryStorage()
        {
            Groups = new Dictionary<string, Core2Group>();
            Users = new Dictionary<string, Schema.Core2EnterpriseUser>();
        }

        private static readonly Lazy<InMemoryStorage> InstanceValue =
            new Lazy<InMemoryStorage>(
                () =>
                    new InMemoryStorage());

        public static InMemoryStorage Instance => InMemoryStorage.InstanceValue.Value;
    }
}
