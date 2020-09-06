using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceReference1;
using WebApiCore.WcfExtensions;

namespace WebApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WcfTestController : ControllerBase
    {
        private readonly WebService1SoapClient _soapClient;
        private readonly IHttpMessageHandlerFactory _httpMessageHandlerFactory;
        private readonly IHttpClientFactory _httpClientFactory;
        private static WebService1SoapClient staticClient = new WebService1SoapClient(WebService1SoapClient.EndpointConfiguration.WebService1Soap12);
        private RemoteEndpoints _remoteEndpoints;

        public WcfTestController(WebService1SoapClient soapClient, IOptionsSnapshot<RemoteEndpoints> remoteEndpoints, IHttpMessageHandlerFactory httpMessageHandlerFactory, IHttpClientFactory httpClientFactory)
        {
            _soapClient = soapClient;
            _httpMessageHandlerFactory = httpMessageHandlerFactory;
            _httpClientFactory = httpClientFactory;
            _remoteEndpoints = remoteEndpoints.Value;
            staticClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(_remoteEndpoints.WcfTest);
        }

        [HttpPost]
        public async Task<ActionResult<string>> SayHello(string hello)
        {

            
                var res = await _soapClient.SayHelloAsync(hello);
                return res.Body.SayHelloResult;
            

        }

        [HttpPost("static")]
        public async Task<ActionResult<string>> SayHelloStatic(string hello)
        {

            var res = await staticClient.SayHelloAsync(hello);
            return res.Body.SayHelloResult;

        }
    }
}