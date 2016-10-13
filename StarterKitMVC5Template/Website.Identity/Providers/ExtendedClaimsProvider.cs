using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Website.Foundation.Core.Aggregates.Identity;
using $safeprojectname$.Constants.Claims;

namespace $safeprojectname$.Providers
{
    public static class ExtendedClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(ApplicationUser user)
        {

            List<Claim> claims = new List<Claim>();

            if (user.PhoneNumberConfirmed)
            {
                claims.Add(CreateClaim(PhoneNumberConfirmed.CLAIM_TYPE, PhoneNumberConfirmed.CLAIM_VALUE.TRUE));

            }
            else
            {
                claims.Add(CreateClaim(PhoneNumberConfirmed.CLAIM_TYPE, PhoneNumberConfirmed.CLAIM_VALUE.FALSE));
            }

            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }

    }
}