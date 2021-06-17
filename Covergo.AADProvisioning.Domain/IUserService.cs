using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoverGo.DomainUtils;

namespace Covergo.AADProvisioning.Domain
{
    public class Policy : SystemObject
    {
        public string Id { get; set; }

        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
    }
    public interface IUserService
    {
        Task<IEnumerable<string>> GetIdsAsync(string tenantId, QueryArguments queryArguments);
        Task<IEnumerable<Policy>> GetPoliciesAsync(string tenantId, QueryArguments queryArguments);

        Task<Result> DeletePolicyAsync(string tenantId, string id, string deletedById);
        Task<Result> SendUpdateCommandAsync(string tenantId, string id, UpdatePolicyCommand command);
    }

    public class UpdatePolicyCommand
    {
        public string Status { get; set; }
        public bool IsStatusChanged { get; set; }
    }
    public class PolicyWhere : Where
    {
        public IEnumerable<PolicyWhere> And { get; set; }
        public IEnumerable<PolicyWhere> Or { get; set; }

        public EntityWhere ContractHolder { get; set; }
        public bool? Status_exists { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? EndDate_lt { get; set; }
        public List<string> Status_in { get; set; }
        public List<string> Id_in { get; set; }
    }

    public class EntityWhere : Where
    {
        public List<string> Id_in { get; set; }
    }

    public enum PolicyStatus
    {
        ISSUED,
        EXPIRED
    }
}
