using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Affecto.Authentication.Claims;

namespace Affecto.AuthenticationServer.Claims
{
    public class AuthenticatedUserFactory : AuthenticatedUserFactory<AuthenticatedUser>
    {
        public AuthenticatedUserFactory(IEnumerable<Claim> claims)
            : base(claims)
        {
        }

        protected override AuthenticatedUser CreateInstance(Guid id, string name, string accountName, IEnumerable<string> permissions = null, IEnumerable<string> roles = null,
            IEnumerable<CustomProperty> customProperties = null, IEnumerable<Guid> groups = null, IEnumerable<string> organizations = null)
        {
            return new AuthenticatedUser(id, name, accountName, permissions, roles, customProperties, groups, organizations);
        }
    }

    public abstract class AuthenticatedUserFactory<T> where T : AuthenticatedUser
    {
        private readonly List<Claim> claims;

        protected AuthenticatedUserFactory(IEnumerable<Claim> claims)
        {
            if (claims == null)
            {
                throw new ArgumentNullException("claims");
            }
            this.claims = claims.ToList();
        }

        public T Create()
        {
            string idStringValue = GetSingleClaimValue(ClaimType.Id);
            Guid id = Guid.Parse(idStringValue);
            string name = GetSingleClaimValue(ClaimType.Name);
            string accountName = GetSingleClaimValue(ClaimType.AccountName);
            List<string> permissions = GetClaimValues(ClaimType.Permission);
            List<string> roles = GetClaimValues(ClaimType.Role);
            List<CustomProperty> customProperties = GetCustomPropertyClaimValues();
            List<Guid> groups = GetGroupClaimValues();
            List<string> organizations = GetClaimValues(ClaimType.Organization);

            return CreateInstance(id, name, accountName, permissions, roles, customProperties, groups, organizations);
        }

        protected abstract T CreateInstance(Guid id, string name, string accountName, IEnumerable<string> permissions = null, IEnumerable<string> roles = null,
            IEnumerable<CustomProperty> customProperties = null, IEnumerable<Guid> groups = null, IEnumerable<string> organizations = null);

        protected string GetSingleClaimValue(string type)
        {
            List<Claim> foundClaims = claims.Where(c => c.Type == type).ToList();

            if (foundClaims.Count == 0)
            {
                return null;
            }
            if (foundClaims.Count > 1)
            {
                throw new MultipleClaimsFoundException(type);
            }

            return foundClaims[0].Value;
        }

        protected List<CustomProperty> GetCustomPropertyClaimValues()
        {
            return claims
                .Where(c => c.Type.StartsWith(ClaimTypePrefix.CustomProperty))
                .Select(c => new CustomProperty(GetCustomPropertyName(c.Type), c.Value))
                .ToList();
        }

        protected List<Guid> GetGroupClaimValues()
        {
            return claims
                .Where(c => c.Type == ClaimType.Group)
                .Select(c => Guid.Parse(c.Value))
                .ToList();
        }

        protected List<string> GetClaimValues(string type)
        {
            return claims
                .Where(c => c.Type == type)
                .Select(c => c.Value)
                .ToList();
        }

        protected string GetCustomPropertyName(string claimType)
        {
            return claimType.Replace(ClaimTypePrefix.CustomProperty, string.Empty);
        }
    }
}