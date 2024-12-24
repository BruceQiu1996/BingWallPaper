using Autofac;
using Autofac.Extensions.DependencyInjection;
using BingWallPaper.Server.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Serilog;

namespace BingWallPaper.Server
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>((hcontext, builder) =>
            {
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
                services.AddEfCoreContext(hostContext.Configuration);
                services.AddControllers();
                services.AddEndpointsApiExplorer();
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
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(app.Configuration.GetSection("FileStorage:AvatarImagesLocation").Value!),
                RequestPath = new PathString("/api/avatars"),
                EnableDirectoryBrowsing = false
            });

            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(app.Configuration.GetSection("FileStorage:RecordImagesLocation").Value!),
                RequestPath = new PathString("/api/record/images"),
                EnableDirectoryBrowsing = false
            });

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
