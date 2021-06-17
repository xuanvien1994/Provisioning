using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covergo.AADProvisioning.Domain;
using Covergo.AADProvisioning.Domain.User;
using Microsoft.SystemForCrossDomainIdentityManagement;
using Core2EnterpriseUser = Covergo.AADProvisioning.Schema.Core2EnterpriseUser;
using ProviderBase = Covergo.AADProvisioning.Service.ProviderBase;
using Resource = Covergo.AADProvisioning.Schema.Resource;
using SchemaIdentifiers = Covergo.AADProvisioning.Schema.SchemaIdentifiers;

namespace Covergo.AADProvisioning.Provider
{
    public class InMemoryProvider: ProviderBase
    {
        private readonly ProviderBase groupProvider;
        private readonly ProviderBase userProvider;
        private readonly IUserService _policyService;
        private readonly IEntityService<Domain.Individual> _entityService;

        public InMemoryProvider(IUserService policyService, IEntityService<Domain.Individual> entityService)
        {
            _policyService = policyService;
            _entityService = entityService;
            this.groupProvider = new InMemoryGroupProvider();
            this.userProvider = new InMemoryUserProvider(policyService, entityService);
        }

        public override Task<Resource> CreateAsync(Resource resource, string correlationIdentifier)
        {
            if (resource is Core2EnterpriseUser)
            {
                return this.userProvider.CreateAsync(resource, correlationIdentifier);
            }

            if (resource is Core2Group)
            {
                return this.groupProvider.CreateAsync(resource, correlationIdentifier);
            }

            throw new NotImplementedException();
        }

        public override Task DeleteAsync(IResourceIdentifier resourceIdentifier, string correlationIdentifier)
        {
            if (resourceIdentifier.SchemaIdentifier.Equals(SchemaIdentifiers.Core2EnterpriseUser))
            {
                return this.userProvider.DeleteAsync(resourceIdentifier, correlationIdentifier);
            }

            if (resourceIdentifier.SchemaIdentifier.Equals(SchemaIdentifiers.Core2Group))
            {
                return this.groupProvider.DeleteAsync(resourceIdentifier, correlationIdentifier);
            }

            throw new NotImplementedException();
        }

        public override Task<Resource> RetrieveAsync(IResourceRetrievalParameters parameters, string correlationIdentifier)
        {
            if (parameters.SchemaIdentifier.Equals(SchemaIdentifiers.Core2EnterpriseUser))
            {
                return this.userProvider.RetrieveAsync(parameters, correlationIdentifier);
            }

            if (parameters.SchemaIdentifier.Equals(SchemaIdentifiers.Core2Group))
            {
                return this.groupProvider.RetrieveAsync(parameters, correlationIdentifier);
            }

            throw new NotImplementedException();
        }

        public override Task<Resource[]> QueryAsync(IQueryParameters parameters, string correlationIdentifier)
        {
            if (parameters.SchemaIdentifier.Equals(SchemaIdentifiers.Core2EnterpriseUser))
            {
                return this.userProvider.QueryAsync(parameters, correlationIdentifier);
            }

            if (parameters.SchemaIdentifier.Equals(SchemaIdentifiers.Core2Group))
            {
                return this.groupProvider.QueryAsync(parameters, correlationIdentifier);
            }

            throw new NotImplementedException();
        }
        public override Task UpdateAsync(IPatch patch, string correlationIdentifier)
        {
            if (patch == null)
            {
                throw new ArgumentNullException(nameof(patch));
            }

            if (string.IsNullOrWhiteSpace(patch.ResourceIdentifier.Identifier))
            {
                throw new ArgumentException(nameof(patch));
            }

            if (string.IsNullOrWhiteSpace(patch.ResourceIdentifier.SchemaIdentifier))
            {
                throw new ArgumentException(nameof(patch));
            }

            if (patch.ResourceIdentifier.SchemaIdentifier.Equals(SchemaIdentifiers.Core2EnterpriseUser))
            {
                return this.userProvider.UpdateAsync(patch, correlationIdentifier);
            }

            if (patch.ResourceIdentifier.SchemaIdentifier.Equals(SchemaIdentifiers.Core2Group))
            {
                return this.groupProvider.UpdateAsync(patch, correlationIdentifier);
            }

            throw new NotImplementedException();
        }
    }
}
