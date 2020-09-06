using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ServiceReference1;
using WebApiCore.WcfExtensions;

namespace WebApiCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //ThreadPool.SetMinThreads(128, 256);
            //ThreadPool.SetMaxThreads(512, 512);

            services.AddControllers();
            services.AddHttpClient("WcfClient", client =>
            {
                //client.Timeout = TimeSpan.FromSeconds(10);
            });
            services.AddHttpClient<HttpClientEndpointBehavior>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "WCF Test API",
                    Version = "v1",
                    Description = "WCF Wrapper API testing"
                });
            });

            services.Configure<RemoteEndpoints>(Configuration.GetSection("RemoteEndpoints"));

            services.AddSingleton<WebService1SoapClient>(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var messageHandlerFactory = serviceProvider.GetRequiredService<IHttpMessageHandlerFactory>();
                RemoteEndpoints remoteEndpoints = serviceProvider.GetRequiredService<IOptionsSnapshot<RemoteEndpoints>>().Value;
                WebService1SoapClient soapClient = new WebService1SoapClient(WebService1SoapClient.EndpointConfiguration.WebService1Soap12, remoteEndpoints.WcfTest);
                soapClient.Endpoint.EndpointBehaviors.Add(new HttpClientEndpointBehavior(messageHandlerFactory, httpClientFactory, clientName: "WcfClient"));
                return soapClient;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ServiceProviderHolder.ServiceProvider = app.ApplicationServices;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "WCF Test API");
                options.DisplayRequestDuration();
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
