using Microsoft.AspNetCore.Identity;
using Social.Network.Domain.Entities;
using Social.Network.SeedWorks.Factories;
using Social.Network.SeedWorks.Interfaces;
using Social.Network.SeedWorks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Social.Network.SeedWorks.Helpers
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;


        public TokenGenerator(IJwtFactory jwtFactory)
        {
            _jwtFactory = jwtFactory;
            _jwtOptions = Program.AppSettingsInfo.JwtIssuerOptions;
        }

        public JwToken TokenGeneration(User user, IList<IdentityRole> userRoles)
        {
            string refreshToken;
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken = Convert.ToBase64String(randomNumber);
            }

            var identity = _jwtFactory.GenerateClaimsIdentity(user.UserName, user.Id);
            if (identity == null)
            {
                throw new SystemException("An error occurred while calling and matching the account information!");
            }

            var userRoleNames = userRoles != null ? userRoles.Select(c => c.Name).ToList() : null;
            var userRoleIds = userRoles != null ? userRoles.Select(c => c.Id.ToString()).ToList() : null;

            var generatedToken = GenerateJwt(user, userRoleNames, userRoleIds, identity, _jwtFactory, refreshToken, _jwtOptions.ExpireTimeTokenInMinute);

            return generatedToken;
        }

        public static JwToken GenerateJwt(User user, IList<string> userRoles, IReadOnlyCollection<string> userRoleIds, ClaimsIdentity identity,
            IJwtFactory jwtFactory, string refreshToken, int refreshTime)
        {
            var result = new JwToken
            {
                TokenType = "Bearer",
                AuthToken = jwtFactory.GenerateEncodedToken(user, userRoles, userRoleIds, identity),
                RefreshToken = refreshToken,
                ExpiresIn = refreshTime,
            };

            return result;
        }

    }
}
