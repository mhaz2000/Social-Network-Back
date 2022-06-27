using Microsoft.AspNetCore.Identity;
using Social.Network.Domain.Entities;
using Social.Network.SeedWorks.Models;
using System.Collections.Generic;

namespace Social.Network.SeedWorks.Interfaces
{
    public interface ITokenGenerator
    {
        JwToken TokenGeneration(User user, IList<IdentityRole> userRoles);
    }
}
