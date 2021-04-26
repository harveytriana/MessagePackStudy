// =============================
// BlazorSpread.net Sample
// =============================
using MessagePack.AspNetCoreMvcFormatter;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MessagePackApi
{
    public class Startup
    {
        readonly string _CorsPolicy = "CorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // the input and output will be in MessagePack binary format
            services.AddMvc().AddMvcOptions(option => {
                option.OutputFormatters.Clear();
                option.OutputFormatters.Add(new MessagePackOutputFormatter(ContractlessStandardResolver.Options));
                option.InputFormatters.Clear();
                option.InputFormatters.Add(new MessagePackInputFormatter(ContractlessStandardResolver.Options));
            });

            services.AddControllers();

            // CORS to allow Blazor client or others
            services.AddCors(options => {
                options.AddPolicy(name: _CorsPolicy,
                    builder => {
                        builder.WithOrigins("https://localhost:44342") // blazor sample
                               .AllowAnyHeader()
                               .AllowCredentials()
                               .AllowAnyMethod()
                               .SetIsOriginAllowedToAllowWildcardSubdomains();
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(_CorsPolicy);
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseDefaultFiles();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            app.Run(async context => {
                await context.Response.WriteAsync("Server is ready.");
            });

        }
    }
}
