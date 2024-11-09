using Microsoft.AspNetCore.Authorization;

namespace Api.App.Features.Policies;

public class RuleClaimRequirement(string claimType, string claimValue): IAuthorizationRequirement
{
    public string ClaimType { get; } = claimType;
    public string ClaimValue { get; } = claimValue;
}