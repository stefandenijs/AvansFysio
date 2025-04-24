using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Core.DomainServices.Authorization
{
    public class WorkerEntryHandler : AuthorizationHandler<WorkerEntryRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, WorkerEntryRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == "Claim.Physiotherapist" || c.Type == "Claim.Intern"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
