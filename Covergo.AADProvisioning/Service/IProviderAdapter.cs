using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.SystemForCrossDomainIdentityManagement;
using QueryResponseBase = Covergo.AADProvisioning.Protocal.QueryResponseBase;
using Resource = Covergo.AADProvisioning.Schema.Resource;

namespace Covergo.AADProvisioning.Service
{
    public interface IProviderAdapter<T> where T : Resource
    {
        string SchemaIdentifier { get; }

        Task<Resource> Create(HttpRequestMessage request, Resource resource, string correlationIdentifier);
        Task Delete(HttpRequestMessage request, string identifier, string correlationIdentifier);
        Task<QueryResponseBase> Query(
            HttpRequestMessage request,
            IReadOnlyCollection<IFilter> filters,
            IReadOnlyCollection<string> requestedAttributePaths,
            IReadOnlyCollection<string> excludedAttributePaths,
            IPaginationParameters paginationParameters,
            string correlationIdentifier);
        Task<Resource> Replace(HttpRequestMessage request, Resource resource, string correlationIdentifier);
        Task<Resource> Retrieve(
            HttpRequestMessage request,
            string identifier,
            IReadOnlyCollection<string> requestedAttributePaths,
            IReadOnlyCollection<string> excludedAttributePaths,
            string correlationIdentifier);
        Task Update(
            HttpRequestMessage request,
            string identifier,
            PatchRequestBase patchRequest,
            string correlationIdentifier);
    }
}
