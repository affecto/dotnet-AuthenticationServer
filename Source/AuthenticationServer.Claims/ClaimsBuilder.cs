using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Affecto.Authentication.Claims;

namespace Affecto.AuthenticationServer.Claims
{
    public class ClaimsBuilder
    {
        private readonly List<Claim> claims;

        public ClaimsBuilder()
        {
            claims = new List<Claim>();
        }

        public ClaimsBuilder SetId(Guid value)
        {
            string stringValue = value.ToString("D");
            UpdateClaim(ClaimType.Id, stringValue);
            return this;
        }

        public ClaimsBuilder SetName(string value)
        {
            UpdateClaim(ClaimType.Name, value);
            return this;
        }

        public ClaimsBuilder SetAccountName(string value)
        {
            UpdateClaim(ClaimType.AccountName, value);
            return this;
        }

        public ClaimsBuilder AddCustomProperty(string name, string value)
        {
            string claimType = string.Format("{0}{1}", ClaimTypePrefix.CustomProperty, name);
            AddClaim(claimType, value);
            return this;
        }

        public ClaimsBuilder AddRole(string value)
        {
            AddClaim(ClaimType.Role, value);
            return this;
        }

        public ClaimsBuilder AddGroup(Guid id)
        {
            AddClaim(ClaimType.Group, id.ToString("D"));
            return this;
        }

        public ClaimsBuilder AddPermission(string value)
        {
            AddClaim(ClaimType.Permission, value);
            return this;
        }

        public ClaimsBuilder AddOrganization(string value)
        {
            AddClaim(ClaimType.Organization, value);
            return this;
        }

        public IReadOnlyCollection<Claim> GetClaims()
        {
            return claims.ToList();
        }

        private void UpdateClaim(string type, string value)
        {
            Claim claim = claims.SingleOrDefault(c => c.Type == type);
            if (claim != null)
            {
                claims.Remove(claim);
            }

            AddClaim(type, value);
        }

        private void AddClaim(string type, string value)
        {
            claims.Add(new Claim(type, value));
        }
    }
}