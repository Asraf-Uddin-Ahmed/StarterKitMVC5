using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Website.Identity.Constants;

namespace $safeprojectname$.Configuration.Identity
{
    public class ChallengeResult : IHttpActionResult
    {
        public ExternalLoginProviderName LoginProvider { get; set; }
        public HttpRequestMessage Request { get; set; }

        public ChallengeResult(ExternalLoginProviderName loginProvider, ApiController controller)
        {
            LoginProvider = loginProvider;
            Request = controller.Request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            Request.GetOwinContext().Authentication.Challenge(LoginProvider.ToString());

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            return Task.FromResult(response);
        }
    }
}
