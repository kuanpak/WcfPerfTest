using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using WebApiCore;
using WebApiCore.WcfExtensions;

namespace ServiceReference1
{
    partial class WebService1SoapClient
    {
        static partial void ConfigureEndpoint(ServiceEndpoint serviceEndpoint, ClientCredentials clientCredentials)
        {
            //var handlerFactory = ServiceProviderHolder.ServiceProvider.GetRequiredService<IHttpMessageHandlerFactory>();
            //serviceEndpoint.EndpointBehaviors.Add(new HttpClientEndpointBehavior(handlerFactory));
        }
    }
}
