﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SystemForCrossDomainIdentityManagement;

namespace Covergo.AADProvisioning.Schema
{
    public static class SchemaIdentifiers
    {
        public const string Extension = "extension:";

        private const string ExtensionEnterprise2 = SchemaIdentifiers.Extension + "enterprise:2.0:";

        public const string None = "/";

        public const string PrefixTypes1 = "urn:scim:schemas:";
        private const string PrefixTypes2 = "urn:ietf:params:scim:schemas:";

        private const string VersionSchemasCore2 = "core:2.0:";

        public const string Core2EnterpriseUser =
            SchemaIdentifiers.PrefixTypes2 +
            SchemaIdentifiers.ExtensionEnterprise2 +
            Types.User;

        public const string Core2Group =
            SchemaIdentifiers.PrefixTypes2 +
            SchemaIdentifiers.VersionSchemasCore2 +
            Types.Group;

        public const string Core2ResourceType =
            SchemaIdentifiers.PrefixTypes2 +
            SchemaIdentifiers.ExtensionEnterprise2 +
            Types.ResourceType;

        public const string Core2ServiceConfiguration =
            SchemaIdentifiers.PrefixTypes2 +
            SchemaIdentifiers.VersionSchemasCore2 +
            Types.ServiceProviderConfiguration;

        public const string Core2User =
            SchemaIdentifiers.PrefixTypes2 +
            SchemaIdentifiers.VersionSchemasCore2 +
            Types.User;

        public const string PrefixExtension =
            SchemaIdentifiers.PrefixTypes2 +
            SchemaIdentifiers.Extension;

    }
}
