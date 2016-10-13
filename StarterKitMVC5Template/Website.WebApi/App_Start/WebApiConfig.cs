using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using $safeprojectname$.Configuration;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using PartialResponse.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace $safeprojectname$.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();

            httpConfig.Formatters.Clear();
            PartialJsonMediaTypeFormatter partialJsonMediaTypeFormatter = new PartialJsonMediaTypeFormatter() { IgnoreCase = true };
            partialJsonMediaTypeFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //partialJsonMediaTypeFormatter.MediaTypeMappings.Add(new QueryStringMapping("type", "json", new MediaTypeHeaderValue("application/json")));
            httpConfig.Formatters.Add(partialJsonMediaTypeFormatter);
            XmlMediaTypeFormatter xmlMediaTypeFormatter = new XmlMediaTypeFormatter();
            //xmlMediaTypeFormatter.MediaTypeMappings.Add(new QueryStringMapping("type", "xml", new MediaTypeHeaderValue("application/xml")));
            httpConfig.Formatters.Add(xmlMediaTypeFormatter);

            httpConfig.MessageHandlers.Add(new XHttpMethodOverrideHandler());
            httpConfig.Services.Add(typeof(IExceptionLogger), new UnhandledExceptionLogger());

            httpConfig.MapHttpAttributeRoutes();
            httpConfig.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            httpConfig.EnableCors();
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            //app.UseWebApi(httpConfig);
            app.UseNinjectMiddleware(() => NinjectConfig.CreateKernel.Value);
            app.UseNinjectWebApi(httpConfig);
        }
    }
}