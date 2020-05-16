using ManageRates.AspnetCore;
using ManageRates.AspnetCore.Builder;
using ManageRates.Core;
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();


            // Register rate strictions services.
            services.AddRateStrictions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            // Attention! RateStrictions middleware should be places between UseRouting and UseEndpoints.
            app.UseMiddleware<ManageRatesMiddleware>();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapGet("/_info", context => context.Response.WriteAsync("success"))
                    // Limit for each IP take information.
                    .ManageRatesByIp(1, RatesStrictPeriod.Minute);

                endpoints.MapControllers();
            });
        }
    }
}
