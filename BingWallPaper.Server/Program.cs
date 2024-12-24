using Autofac;
using Autofac.Extensions.DependencyInjection;
using BingWallPaper.Server.Database;
using BingWallPaper.Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

namespace BingWallPaper.Server
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>((hcontext, builder) =>
            {
                builder.AddApplicationContainer(Assembly.GetExecutingAssembly());
            });

            builder.Host.ConfigureServices((hostContext, services) =>
            {
                //配置跨域
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
                });
                services.AddHostedService<BingWallPaperCrawDaliyService>();
                services.AddEfCoreContext(hostContext.Configuration);
                services.AddControllers();
                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "bingwallpaper",
                        Description = "bingwallpaper v1版本接口"
                    });
                });
            }).UseSerilog((context, logger) =>
            {
                logger.WriteTo.Console();
            });
            builder.Services.AddControllers();
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var app = builder.Build();
            app.Urls.Add("http://*:5000");
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Add("Cache-Control", "public,max-age=600");
                }
            });

            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint($"/swagger/v1/swagger.json", "v1");
            });
            app.MapControllers();

            app.Run();
        }
    }
}
