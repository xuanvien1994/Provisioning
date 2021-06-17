using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SystemForCrossDomainIdentityManagement;

namespace Covergo.AADProvisioning.Service
{
    internal class Core2EnterpriseUserProviderAdapter : ProviderAdapterTemplate<Schema.Core2EnterpriseUser>
    {
        public Core2EnterpriseUserProviderAdapter(Service.IProvider provider)
            : base(provider)
        {
        }

        public override string SchemaIdentifier
        {
            get
            {
                return SchemaIdentifiers.Core2EnterpriseUser;
            }
        }
    }
}
