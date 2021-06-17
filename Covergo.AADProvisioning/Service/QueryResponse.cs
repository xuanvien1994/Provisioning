using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Covergo.AADProvisioning.Protocal;
using Resource = Covergo.AADProvisioning.Schema.Resource;

namespace Covergo.AADProvisioning.Service
{
    [DataContract]
    public sealed class QueryResponse : QueryResponseBase
    {
        public QueryResponse()
            : base()
        {
        }

        public QueryResponse(IReadOnlyCollection<Resource> resources)
            : base(resources)
        {
        }

        public QueryResponse(IList<Resource> resources)
            : base(resources)
        {
        }
    }
}
