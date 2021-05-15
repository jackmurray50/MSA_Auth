using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MSA_Auth;

namespace MSA_Auth_API.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddAccountContext(this IServiceCollection services, string connectionString)
        {
            return services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<AccountContext>(opt =>
                {
                    opt.UseSqlServer(
                        connectionString,
                        x =>
                        {
                            x.MigrationsAssembly(typeof(Startup)
                                .GetTypeInfo()
                                .Assembly
                                .GetName().Name);
                        });
                });
        }
    }
}
