using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covergo.AADProvisioning.Domain;
using Covergo.AADProvisioning.Domain.User;
using CoverGo.DomainUtils;
using Microsoft.SystemForCrossDomainIdentityManagement;
using Individual = Covergo.AADProvisioning.Domain.Individual;

namespace Covergo.AADProvisioning.Provider
{
    public class InMemoryUserProvider : Service.ProviderBase
    {
        private readonly InMemoryStorage _storage;
        private readonly IUserService _policyService;
        private readonly IEntityService<Individual> _entityService;

        public InMemoryUserProvider(IUserService policyService, IEntityService<Individual> entityService)
        {
            _policyService = policyService;
            this._storage = InMemoryStorage.Instance;
            _entityService = entityService;
        }

        public override async Task<Schema.Resource> CreateAsync( Schema.Resource resource, string correlationIdentifier)
        {
            if (resource.Identifier != null)
            {
                throw new NotImplementedException();
            }

            Schema.Core2EnterpriseUser user = resource as Schema.Core2EnterpriseUser;
            if (string.IsNullOrWhiteSpace(user?.UserName))
            {
                throw new NotImplementedException();
            }

            IEnumerable<Schema.Core2EnterpriseUser> existingUsers = _storage.Users.Values;
            if
            (
                existingUsers.Any(
                    existingUser =>
                        string.Equals(existingUser.UserName, user.UserName, StringComparison.Ordinal))
            )
            {
                throw new NotImplementedException();
            }

            string resourceIdentifier = Guid.NewGuid().ToString();
            resource.Identifier = resourceIdentifier;
            _storage.Users.Add(resourceIdentifier, user);
            DateTime dateTimeToCheck = DateTime.UtcNow.Date;


            Result<CreatedStatus> createdIndividualResult = await _entityService.CreateAsync("tcb_uat", new CreateIndividualCommand()
            {
                EnglishFirstName = user.Name.GivenName,
                EnglishLastName = user.Name.FamilyName,
                InternalCodeLength =3,
                Type = IndividualTypes.Customer
            });

            return resource;
        }

        public override Task DeleteAsync(IResourceIdentifier resourceIdentifier, string correlationIdentifier)
        {
            if (string.IsNullOrWhiteSpace(resourceIdentifier?.Identifier))
            {
                throw new NotImplementedException();
            }

            string identifier = resourceIdentifier.Identifier;

            if (_storage.Users.ContainsKey(identifier))
            {
                _storage.Users.Remove(identifier);
            }

            return Task.CompletedTask;
        }

        public override Task<Schema.Resource[]> QueryAsync(IQueryParameters parameters, string correlationIdentifier)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (string.IsNullOrWhiteSpace(correlationIdentifier))
            {
                throw new ArgumentNullException(nameof(correlationIdentifier));
            }

            if (null == parameters.AlternateFilters)
            {
                throw new ArgumentException("SampleServiceResources.ExceptionInvalidParameters");
            }

            if (string.IsNullOrWhiteSpace(parameters.SchemaIdentifier))
            {
                throw new ArgumentException("SampleServiceResources.ExceptionInvalidParameters");
            }

            Schema.Resource[] results;
            IFilter queryFilter = parameters.AlternateFilters.SingleOrDefault();
            if (queryFilter == null)
            {
                IEnumerable<Schema.Core2EnterpriseUser> allUsers = _storage.Users.Values;
                results =
                    allUsers.Select(user => user as Schema.Resource).ToArray();

                return Task.FromResult(results);
            }

            if (string.IsNullOrWhiteSpace(queryFilter.AttributePath))
            {
                throw new ArgumentException("SampleServiceResources.ExceptionInvalidParameters");
            }

            if (string.IsNullOrWhiteSpace(queryFilter.ComparisonValue))
            {
                throw new ArgumentException("SampleServiceResources.ExceptionInvalidParameters");
            }

            if (queryFilter.FilterOperator != ComparisonOperator.Equals)
            {
                throw new NotSupportedException("SampleServiceResources.UnsupportedComparisonOperator");
            }

            if (queryFilter.AttributePath.Equals(AttributeNames.UserName))
            {
                IEnumerable<Schema.Core2EnterpriseUser> allUsers = _storage.Users.Values;
                results =
                    allUsers.Where(
                        item =>
                           string.Equals(
                                item.UserName,
                               parameters.AlternateFilters.Single().ComparisonValue,
                               StringComparison.OrdinalIgnoreCase))
                               .Select(user => user as Schema.Resource).ToArray();

                return Task.FromResult(results);
            }

            if (queryFilter.AttributePath.Equals(AttributeNames.ExternalIdentifier))
            {
                IEnumerable<Schema.Core2EnterpriseUser> allUsers = this._storage.Users.Values;
                results =
                    allUsers.Where(
                        item =>
                           string.Equals(
                                item.ExternalIdentifier,
                               parameters.AlternateFilters.Single().ComparisonValue,
                               StringComparison.OrdinalIgnoreCase))
                               .Select(user => user as Schema.Resource).ToArray();

                return Task.FromResult(results);
            }

            throw new NotSupportedException("SampleServiceResources.UnsupportedFilterAttributeUser");
        }

        public override Task<Schema.Resource> ReplaceAsync(Schema.Resource resource, string correlationIdentifier)
        {
            if (resource.Identifier == null)
            {
                throw new NotImplementedException();
            }

            var user = resource as Schema.Core2EnterpriseUser;

            if (string.IsNullOrWhiteSpace(user?.UserName))
            {
                throw new NotImplementedException();
            }

            IEnumerable<Schema.Core2EnterpriseUser> existingUsers = _storage.Users.Values;
            if
            (
                existingUsers.Any(
                    existingUser =>
                        string.Equals(existingUser.UserName, user.UserName, StringComparison.Ordinal) &&
                        !string.Equals(existingUser.Identifier, user.Identifier, StringComparison.OrdinalIgnoreCase))
            )
            {
                throw new NotImplementedException();
            }

            if (!_storage.Users.TryGetValue(user.Identifier, out Schema.Core2EnterpriseUser _))
            {
                throw new NotImplementedException();
            }


            _storage.Users[user.Identifier] = user;
            Schema.Resource result = (Schema.Resource) user;

            return Task.FromResult(result);
        }

        public override Task<Schema.Resource> RetrieveAsync(IResourceRetrievalParameters parameters, string correlationIdentifier)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (string.IsNullOrWhiteSpace(correlationIdentifier))
            {
                throw new ArgumentNullException(nameof(correlationIdentifier));
            }

            if (string.IsNullOrEmpty(parameters?.ResourceIdentifier?.Identifier))
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            string identifier = parameters.ResourceIdentifier.Identifier;

            if (!_storage.Users.ContainsKey(identifier)) throw new NotImplementedException();
            if (!_storage.Users.TryGetValue(identifier, out Schema.Core2EnterpriseUser user))
                throw new NotImplementedException();
            var result = user as Schema.Resource;
            return Task.FromResult(result);

        }

        public override Task UpdateAsync(IPatch patch, string correlationIdentifier)
        {
            if (null == patch)
            {
                throw new ArgumentNullException(nameof(patch));
            }

            if (null == patch.ResourceIdentifier)
            {
                throw new ArgumentException("SampleServiceResources.ExceptionInvalidPatch");
            }

            if (string.IsNullOrWhiteSpace(patch.ResourceIdentifier.Identifier))
            {
                throw new ArgumentException("SampleServiceResources.ExceptionInvalidPatch");
            }

            if (null == patch.PatchRequest)
            {
                throw new ArgumentException("SampleServiceResources.ExceptionInvalidPatch");
            }

            PatchRequest2 patchRequest =
                patch.PatchRequest as PatchRequest2;

            if (null == patchRequest)
            {
                string unsupportedPatchTypeName = patch.GetType().FullName;
                throw new NotSupportedException(unsupportedPatchTypeName);
            }

            if (_storage.Users.TryGetValue(patch.ResourceIdentifier.Identifier, out Schema.Core2EnterpriseUser user))
            {
                //user.Apply(patchRequest);
            }
            else
            {
                throw new NotImplementedException();
            }

            return Task.CompletedTask;
        }
        
    }
}
