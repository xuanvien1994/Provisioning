using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Covergo.AADProvisioning.Domain;
using CoverGo.DomainUtils;
using CoverGo.HttpUtils;
using Newtonsoft.Json;

namespace Covergo.AADProvisioning.Infratructure.Service
{
    public class CoverGoPolicyService : IUserService
    {
        private readonly HttpClient _client;

        public CoverGoPolicyService(HttpClient client)
        {
            _client = client;
        }

        public Task<IEnumerable<string>> GetIdsAsync(string tenantId, QueryArguments queryArguments) =>
            _client.GenericPostAsync<IEnumerable<string>, QueryArguments>($"{tenantId}/api/v1/policies/queryids", queryArguments);

        public Task<IEnumerable<Policy>> GetPoliciesAsync(string tenantId, QueryArguments queryArguments) =>
            _client.GenericPostAsync<IEnumerable<Policy>, QueryArguments>($"{tenantId}/api/v1/policies/query", queryArguments);

        public async Task<Result> DeletePolicyAsync(string tenantId, string id, string deletedById)
        {
            HttpResponseMessage response = await _client.DeleteAsync(
                $"{tenantId}/api/v1/policies/{id}/{deletedById}");

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Result>(json);
        }

        public async Task<Result> SendUpdateCommandAsync(string tenantId, string id, UpdatePolicyCommand command)
        {
            HttpResponseMessage response = await _client.PutAsync(
                $"{tenantId}/api/v1/policies/policies/{id}",
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Result>(json);
        }
    }
}
