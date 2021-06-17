using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.SystemForCrossDomainIdentityManagement;

namespace Covergo.AADProvisioning.Schema
{
    [DataContract]
    public abstract class UserBase : Schema.Resource
    {
        [DataMember(Name = AttributeNames.UserName)]
        public virtual string UserName
        {
            get;
            set;
        }
    }
}
