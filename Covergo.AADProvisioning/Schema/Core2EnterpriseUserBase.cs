using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.SystemForCrossDomainIdentityManagement;

namespace Covergo.AADProvisioning.Schema
{
    [DataContract]
    public abstract class Core2EnterpriseUserBase : Core2UserBase
    {
        protected Core2EnterpriseUserBase()
            : base()
        {
            this.AddSchema(SchemaIdentifiers.Core2EnterpriseUser);
            this.EnterpriseExtension = new ExtensionAttributeEnterpriseUser2();
        }

        [DataMember(Name = AttributeNames.ExtensionEnterpriseUser2)]
        public ExtensionAttributeEnterpriseUser2 EnterpriseExtension
        {
            get;
            set;
        }
    }
}
