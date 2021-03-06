using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SystemForCrossDomainIdentityManagement;

namespace Covergo.AADProvisioning.Service
{
    public static class ServiceConstants
    {
        public const string PathSegmentResourceTypes = "ResourceTypes";
        //public const string PathSegmentToken = "token";
        public const string PathSegmentSchemas = "Schemas";
        public const string PathSegmentServiceProviderConfiguration = "ServiceProviderConfig";

        public const string RouteGroups = SchemaConstants.PathInterface + ServiceConstants.SeparatorSegments + ProtocolConstants.PathGroups;
        public const string RouteResourceTypes = SchemaConstants.PathInterface + ServiceConstants.SeparatorSegments + ServiceConstants.PathSegmentResourceTypes;
        public const string RouteSchemas = SchemaConstants.PathInterface + ServiceConstants.SeparatorSegments + ServiceConstants.PathSegmentSchemas;
        public const string RouteServiceConfiguration = SchemaConstants.PathInterface + ServiceConstants.SeparatorSegments + ServiceConstants.PathSegmentServiceProviderConfiguration;
        //public const string RouteToken = SchemaConstants.PathInterface + ServiceConstants.SeparatorSegments + ServiceConstants.PathSegmentToken;
        public const string RouteUsers = SchemaConstants.PathInterface + ServiceConstants.SeparatorSegments + ProtocolConstants.PathUsers;

        public const string SeparatorSegments = "/";

        //public const string TokenAudience = "Microsoft.Security.Bearer";
        //public const string TokenIssuer = "Microsoft.Security.Bearer";
    }
}
