using System;
using Covergo.AADProvisioning.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SystemForCrossDomainIdentityManagement;
using Core2EnterpriseUser = Covergo.AADProvisioning.Schema.Core2EnterpriseUser;
using IProvider = Covergo.AADProvisioning.Service.IProvider;

namespace Covergo.AADProvisioning.Controllers
{
    [Route(Service.ServiceConstants.RouteUsers)]
    [Authorize]
    [ApiController]
    public sealed class UsersController : ControllerTemplate<Core2EnterpriseUser>
    {
        public UsersController(IProvider provider, IMonitor monitor) : base(provider, monitor)
        {
        }

        protected override Service.IProviderAdapter<Core2EnterpriseUser> AdaptProvider(IProvider provider)
        {
            if (null == provider)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            Service.IProviderAdapter<Core2EnterpriseUser> result = new Core2EnterpriseUserProviderAdapter(provider);
            return result;
        }

    }
}
