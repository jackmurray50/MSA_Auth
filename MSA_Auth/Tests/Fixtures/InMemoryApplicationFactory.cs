using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using MSA_Auth_API.Tests.Logging;

namespace MSA_Auth_API.Tests.Fixtures
{
    public class InMemoryApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private ITestOutputHelper _testOutputHelper;

        public void SetTestOutputHelper(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Testing")
                .ConfigureTestServices(services =>
                {
                    var options = new DbContextOptionsBuilder<AccountContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .Options;

                    if (_testOutputHelper != null)
                    {
                        services.AddLogging(cfg => cfg.AddProvider(new TestOutputLoggerProvider(_testOutputHelper)));
                    }

                    services.AddScoped<AccountContext>(serviceProvider => new TestAccountContext(options));
                    services.Replace(ServiceDescriptor.Scoped(_ => new AccountContextFactory().InMemoryAccountManager));
                    services.AddSingleton<IDistributedCache, MemoryDistributedCache>();

                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<AccountContext>();
                    db.Database.EnsureCreated();
                });
        }
    }
}   