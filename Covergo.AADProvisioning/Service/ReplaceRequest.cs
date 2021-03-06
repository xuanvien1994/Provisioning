using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.SystemForCrossDomainIdentityManagement;
using Resource = Covergo.AADProvisioning.Schema.Resource;

namespace Covergo.AADProvisioning.Service
{
    public sealed class ReplaceRequest : SystemForCrossDomainIdentityManagementRequest<Resource>
    {
        public ReplaceRequest(
            HttpRequestMessage request,
            Resource payload,
            string correlationIdentifier,
            IReadOnlyCollection<IExtension> extensions)
            : base(request, payload, correlationIdentifier, extensions)
        {
        }
    }
}
