using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.SystemForCrossDomainIdentityManagement;

namespace Covergo.AADProvisioning.Schema
{
    [DataContract]
    public abstract class Resource : Schematized
    {
        [DataMember(Name = AttributeNames.ExternalIdentifier, IsRequired = false, EmitDefaultValue = false)]
        public string ExternalIdentifier
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Identifier)]
        public string Identifier
        {
            get;
            set;
        }

        public virtual bool TryGetIdentifier(Uri baseIdentifier, out Uri identifier)
        {
            identifier = null;
            return false;
        }

        public virtual bool TryGetPathIdentifier(out Uri pathIdentifier)
        {
            pathIdentifier = null;
            return false;
        }
    }
}
