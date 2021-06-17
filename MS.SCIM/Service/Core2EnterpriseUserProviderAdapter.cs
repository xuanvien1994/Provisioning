using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SystemForCrossDomainIdentityManagement;

namespace MS.SCIM.Service
{
    internal class Core2EnterpriseUserProviderAdapter : ProviderAdapterTemplate<Core2EnterpriseUser>
    {
        public Core2EnterpriseUserProviderAdapter(IProvider provider)
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
