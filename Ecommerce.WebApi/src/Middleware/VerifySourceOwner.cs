using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.WebApi.src.Middleware
{
    public class VerifyResourceOwnerRequirement : IAuthorizationRequirement
    {
        public VerifyResourceOwnerRequirement() { }
    }
}
