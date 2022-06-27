﻿using Microsoft.Extensions.Options;
using Social.Network.Domain.Entities;
using Social.Network.SeedWorks.Helpers;
using Social.Network.SeedWorks.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Social.Network.SeedWorks.Factories
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        public string GenerateEncodedToken(User user, IList<string> userRoles, IEnumerable<string> roleIds, ClaimsIdentity identity)
        {
            identity.AddClaim(new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, user.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, userRoles != null ? string.Join(',', userRoles) : ""));
            identity.AddClaim(new Claim("roleIds", userRoles != null ? string.Join(',', roleIds) : ""));
            identity.AddClaim(new Claim("id", user.Id));


            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(),
                ClaimValueTypes.Integer64));

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: identity.Claims,
                notBefore: _jwtOptions.NotBefore,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtTokenHelper().WriteToken(jwt);

            return encodedJwt;
        }

        public ClaimsIdentity GenerateClaimsIdentity(string userName, string id)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim(AuthDataService.JwtClaimIdentifiers.Id, id),
                new Claim(AuthDataService.JwtClaimIdentifiers.Rol, AuthDataService.JwtClaims.ApiAccess)
            });
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date) => (long)Math.Round(
            (date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidTimeInMinute <= 0)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidTimeInMinute));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }
    }
}
