using System.Security.Claims;

namespace Affecto.AuthenticationServer.IdentityManagement
{
    public class ClaimsAuthenticationManager : System.Security.Claims.ClaimsAuthenticationManager
    {
        /// <summary>
        /// When overridden in a derived class, returns a <see cref="T:System.Security.Claims.ClaimsPrincipal"/> object consistent with the requirements of the RP application. The default implementation does not modify the incoming <see cref="T:System.Security.Claims.ClaimsPrincipal"/>.
        /// </summary>
        /// <returns>
        /// A claims principal that contains any modifications necessary for the RP application. The default implementation returns the incoming claims principal unmodified.
        /// </returns>
        /// <param name="resourceName">The address of the resource that is being requested.</param><param name="incomingPrincipal">The claims principal that represents the authenticated user that is attempting to access the resource.</param>
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            return base.Authenticate(resourceName, incomingPrincipal);
        }
    }
}
