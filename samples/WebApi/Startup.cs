using ManageRates.AspnetCore;
using ManageRates.AspnetCore.Builder;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // <configure_services>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Manage rates service uses IMemoryCache, sow we need to add it.
            services.AddMemoryCache();
            // Register rate strictions services.
            services.AddRateStrictions();
        }
        // </configure_services>

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            app.UseRouting();

            // Attention! RateStrictions middleware should be placed between UseRouting and UseEndpoints.
            /*
            app.UseManageRates();
            */
            app.UseManageRates(policyBuilder => policyBuilder
                .AddPolicy("/raw/.*endpoint.*", 2, Period.Second, KeyType.None)
                .AddPolicy("/raw/.*ip.*", 2, Period.Second, KeyType.Ip)
                .AddPolicy("/raw/.*user.*", 2, Period.Second, KeyType.User)
                .AddNamedPolicy("NoMore10PerSecond", 10, Period.Second, KeyType.None)
                
                // sample of your own extractor
                .AddNamedPolicy("userHeader", 3, Period.Second, context => context.Request.Headers["X-userId"])
            );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/endpoint", context => context.Response.WriteAsync("endpoint"))
                    .ManageRates(2, Period.Second, KeyType.RequestPath);

                endpoints.MapGet("/user", context => context.Response.WriteAsync("user"))
                    .ManageRates(2, Period.Second, KeyType.User);

                endpoints.MapGet("/ip", context => context.Response.WriteAsync("ip"))
                    .ManageRates(2, Period.Second, KeyType.Ip);

                endpoints.MapGet("/header", context => context.Response.WriteAsync("header"))
                    .ManageRates("userHeader");

                endpoints.MapGet("/namedpolicy", context => context.Response.WriteAsync("use named policy"))
                    .ManageRates("NoMore10PerSecond");

                endpoints.MapGet("/delegate", context => context.Response.WriteAsync("delegate"))
                    .ManageRates((httpContext, timeService, memoryCache) => new ManageRatesResult(false));

                endpoints.MapControllers();
            });
        }
    }
}
