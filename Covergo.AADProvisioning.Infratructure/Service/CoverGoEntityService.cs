using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Covergo.AADProvisioning.Domain;
using Covergo.AADProvisioning.Domain.User;
using CoverGo.DomainUtils;
using CoverGo.HttpUtils;
using Individual = Covergo.AADProvisioning.Domain.Individual;

namespace Covergo.AADProvisioning.Infratructure.Service
{
    public class CoverGoEntityService<T> : IEntityService<T>
        where T : Entity
    {
        private readonly HttpClient _client;
        private readonly string _type;

        public CoverGoEntityService(HttpClient client)
        {
            _client = client;

            if (typeof(T) == typeof(Individual))
                _type = "individuals";
            //else if (typeof(T) == typeof(Internal))
            //    _type = "internals";
        }

        public Task<IEnumerable<T>> GetAsync(string tenantId, QueryArguments where)
            => _client.GenericPostAsync<IEnumerable<T>, QueryArguments>($"{tenantId}/api/v1/entities/{_type}/query", where);

        public Task<Result<CreatedStatus>> CreateAsync(string tenantId, CreateIndividualCommand command) =>
            _client.GenericPostAsync<Result<CreatedStatus>, CreateIndividualCommand>($"{tenantId}/api/v1/entities/{_type}", command);

        //public Task<Result> UpdateAsync(string tenantId, string entityId, U command) =>
        //    _client.GenericPutAsync<Result, U>($"{tenantId}/api/v1/entities/{_type}/{entityId}", command);


        public Task<IEnumerable<Relationships>> GetRelationshipsAsync(string tenantId, QueryArguments queryArguments)
            => _client.GenericPostAsync<IEnumerable<Relationships>, QueryArguments>($"{tenantId}/api/v1/entities/links/query", queryArguments);

        
    }
}
