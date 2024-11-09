using Microsoft.AspNetCore.Authorization;

namespace Api.App.Features.Policies;

public class RuleClaimHandler: AuthorizationHandler<RuleClaimRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        RuleClaimRequirement requirement)
    {
        if (context.User.HasClaim(c => c.Type == requirement.ClaimType && c.Value == requirement.ClaimValue))
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}