using Social.Network.Domain.Entities;
using System.Collections.Generic;
using System.Security.Claims;

namespace Social.Network.SeedWorks.Factories
{
    public interface IJwtFactory
    {
        string GenerateEncodedToken(User user, IList<string> userRoles, IEnumerable<string> roleIds, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
    }
}
