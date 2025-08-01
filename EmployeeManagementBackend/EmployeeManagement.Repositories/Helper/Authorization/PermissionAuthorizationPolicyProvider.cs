using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace EmployeeManagement.Repositories.Helper.Authorization;

public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
    }

    public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        AuthorizationPolicy policy = await base.GetPolicyAsync(policyName);

        if (policy is not null)
        {
            return policy;
        }

        var parts = policyName.Split('_');
        if (parts.Length == 2)
        {
            var module = parts[0];
            var actionType = parts[1];

            return new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(module, actionType))
                .Build();
        }

        return null;
    }

}
