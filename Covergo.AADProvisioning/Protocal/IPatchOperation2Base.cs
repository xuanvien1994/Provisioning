using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SystemForCrossDomainIdentityManagement;

namespace Covergo.AADProvisioning.Protocal
{
    public interface IPatchOperation2Base
    {
        OperationName Name { get; set; }
        IPath Path { get; set; }
    }
}
