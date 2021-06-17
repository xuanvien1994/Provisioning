using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SystemForCrossDomainIdentityManagement;

namespace Covergo.AADProvisioning.Service
{
    public interface IProvider
    {
        bool AcceptLargeObjects { get; set; }
        ServiceConfigurationBase Configuration { get; }
        //IEventTokenHandler EventHandler { get; set; }
        IReadOnlyCollection<IExtension> Extensions { get; }
        IResourceJsonDeserializingFactory<GroupBase> GroupDeserializationBehavior { get; }
        ISchematizedJsonDeserializingFactory<PatchRequest2> PatchRequestDeserializationBehavior { get; }
        IReadOnlyCollection<Core2ResourceType> ResourceTypes { get; }
        IReadOnlyCollection<TypeScheme> Schema { get; }
        //Action<IApplicationBuilder, HttpConfiguration> StartupBehavior { get; }
        IResourceJsonDeserializingFactory<Core2UserBase> UserDeserializationBehavior { get; }
        Task<Schema.Resource> CreateAsync(IRequest<Schema.Resource> request);
        Task DeleteAsync(IRequest<IResourceIdentifier> request);
        Task<Protocal.QueryResponseBase> PaginateQueryAsync(IRequest<IQueryParameters> request);
        Task<Schema.Resource[]> QueryAsync(IRequest<IQueryParameters> request);
        Task<Schema.Resource> ReplaceAsync(IRequest<Schema.Resource> request);
        Task<Schema.Resource> RetrieveAsync(IRequest<IResourceRetrievalParameters> request);
        Task UpdateAsync(IRequest<IPatch> request);
    }
}
