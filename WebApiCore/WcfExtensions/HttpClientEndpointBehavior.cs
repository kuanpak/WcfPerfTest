using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Threading.Tasks;

namespace WebApiCore.WcfExtensions
{
    public class HttpClientEndpointBehavior : IEndpointBehavior
    {
        private readonly Func<HttpMessageHandler> _httpHandler;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _clientName;

        public HttpClientEndpointBehavior(IHttpMessageHandlerFactory factory, IHttpClientFactory httpClientFactory, string clientName = "WcfClient")
        {
            _httpHandler = () => factory.CreateHandler("WcfClient");
            _httpClientFactory = httpClientFactory;
            _clientName = clientName;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            bindingParameters.Add(_httpClientFactory.CreateClient(_clientName));
            //bindingParameters.Add(new Func<HttpClientHandler, HttpMessageHandler>(clientHandler =>
            //{
            //    clientHandler.MaxConnectionsPerServer = 512;
            //    return _httpHandler();
            //}));

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            
        }
    }
}
