using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using $safeprojectname$.Core.Aggregates;

namespace $safeprojectname$.Core.Identity
{
    public static class ExtendedClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(ApplicationUser user)
        {

            List<Claim> claims = new List<Claim>();

            if (user.PhoneNumberConfirmed)
            {
                claims.Add(CreateClaim("FTE", "1"));

            }
            else
            {
                claims.Add(CreateClaim("FTE", "0"));
            }

            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }

    }
}