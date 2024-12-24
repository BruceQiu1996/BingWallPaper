using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BingWallPaper.Server.Database
{
    public static class ServiceCollectionExtension
    {
        public static void AddEfCoreContext(this IServiceCollection services, IConfiguration configuration)
        {
            var mysqlConfig = configuration.GetSection("Mysql").Get<MysqlOptions>();
            var serverVersion = new MariaDbServerVersion(new Version(8, 0, 29));
            services.AddDbContext<DbContext, BingWallPaperContext>(options =>
            {
                options.UseMySql(mysqlConfig.ConnectionString, serverVersion, optionsBuilder =>
                {
                    optionsBuilder.MinBatchSize(4).UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
            });
        }
    }
}
