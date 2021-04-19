using MessagePack.AspNetCoreMvcFormatter;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

// Good approach
// https://dotnetthoughts.net/using-message-pack-with-asp-net-core/
// https://www.strathweb.com/2017/06/using-messagepack-with-asp-net-core-mvc/
// https://dotnet.libhunt.com/messagepack-csharp-alternatives

namespace MessagePackApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddMvcOptions(option => {
                option.OutputFormatters.Clear();
                option.OutputFormatters.Add(new MessagePackOutputFormatter(ContractlessStandardResolver.Options));
                option.InputFormatters.Clear();
                option.InputFormatters.Add(new MessagePackInputFormatter(ContractlessStandardResolver.Options));
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
