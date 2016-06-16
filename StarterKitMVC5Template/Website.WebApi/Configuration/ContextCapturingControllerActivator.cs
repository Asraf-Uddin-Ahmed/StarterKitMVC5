using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace $safeprojectname$.Configuration
{
    public class ContextCapturingControllerActivator : IHttpControllerActivator
    {
        private readonly IKernel kernel;

        public ContextCapturingControllerActivator(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public IHttpController Create(HttpRequestMessage requestMessage, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            this.kernel.Rebind<HttpRequestMessage>().ToConstant<HttpRequestMessage>(requestMessage);

            var controller = (IHttpController)this.kernel.GetService(controllerType);

            requestMessage.RegisterForDispose(new Release(() => this.kernel.Release(controller)));

            return controller;
        }

        private class Release : IDisposable
        {
            private readonly Action release;

            public Release(Action release)
            {
                this.release = release;
            }

            public void Dispose()
            {
                this.release();
            }
        }
    }
}