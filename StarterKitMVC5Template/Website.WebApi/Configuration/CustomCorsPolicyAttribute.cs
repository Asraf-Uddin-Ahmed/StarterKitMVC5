using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace $safeprojectname$.Configuration
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CustomCorsPolicyAttribute : Attribute, ICorsPolicyProvider
    {
        public async Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var corsRequestContext = request.GetCorsRequestContext();
            var originRequested = corsRequestContext.Origin;
            if (await IsOriginFromAPaidCustomer(originRequested))
            {
                // Grant CORS request
                var policy = new CorsPolicy
                {
                    AllowAnyHeader = true,
                    AllowAnyMethod = true,
                };
                policy.Origins.Add(originRequested);
                return policy;
            }
            else
            {
                // Reject CORS request
                return null;
            }
        }
        private async Task<bool> IsOriginFromAPaidCustomer(string originRequested)
        {
            // Do database look up here to determine if origin should be allowed
            return true;
        }
    }
}