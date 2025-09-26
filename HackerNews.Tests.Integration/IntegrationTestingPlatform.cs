using HackerNews.Domain.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace HackerNews.Tests.Integration
{
    public class IntegrationTestingPlatform<T> : WebApplicationFactory<T> where T : class
    {
        public readonly Mock<HttpMessageHandler>? MessageHandler = new Mock<HttpMessageHandler>();
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                //remove previous http client factory and client
                var factories = services.Where(o => o.ServiceType == typeof(IHttpClientFactory)).ToList();
                foreach (var factory in factories)
                {
                    services.Remove(factory);
                }
                var clients = services.Where(o => o.ServiceType == typeof(HttpClient)).ToList();
                foreach (var client in clients)
                {
                    services.Remove(client);
                }

                //add testing client factory and client
                var fakeClient = new HttpClient(MessageHandler.Object)
                {
                    BaseAddress = new Uri("https://www.test.com")
                };
                var fakeClientFactory = new Mock<IHttpClientFactory>();
                fakeClientFactory
                    .Setup(m => m.CreateClient(It.IsAny<string>()))
                    .Returns(fakeClient);

                services.AddSingleton(fakeClientFactory.Object);
                services.AddTransient(o => fakeClient);
            });
        }
    }
}