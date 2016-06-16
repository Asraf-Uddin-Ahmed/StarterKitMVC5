using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;

namespace $safeprojectname$.Models.Response
{
    public abstract class ResponseModel
    {
        [JsonProperty(Order = -2)]
        public string Url { get; set; }
    }
}