using System.Security.Claims;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.WebApi.src.Authorization
{
    public class AdminOrOwnerRequirement : IAuthorizationRequirement
    {
        public AdminOrOwnerRequirement() { }
    }

    public class AdminOrOwnerHandler : AuthorizationHandler<AdminOrOwnerRequirement, UserReadDto>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AdminOrOwnerRequirement requirement,
            UserReadDto user
        )
        {
            var claims = context.User;
            var userRole = claims.FindFirst(c => c.Type == ClaimTypes.Role)!.Value;
            var userId = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value; // id of logged in user
            Console.WriteLine("Running authorization check =============");
            Console.WriteLine($"user id: {user.Id}");
            Console.WriteLine($"user role: {userRole}");
            if (userRole == UserRole.Admin.ToString() || userId == user.Id.ToString())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
