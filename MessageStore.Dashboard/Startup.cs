using System;
using System.Net.Http;
using System.Net.Http.Headers;
using MessageStore.Dashboard.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessageStore.Dashboard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddCors();

            ApplicationConfiguration configuration = Configuration.GetSection("ApplicationConfiguration")
                .Get<ApplicationConfiguration>();

            services.AddSingleton<IApplicationConfiguration, ApplicationConfiguration>(
                e => configuration);

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var apiEndPoint = new Uri(configuration.MessageStoreApiUrl);
            var httpClient = new HttpClient
            {
                BaseAddress = apiEndPoint,
            };

            httpClient.DefaultRequestHeaders.Clear();  
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            services.AddSingleton<HttpClient>(httpClient);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            ApplicationConfiguration configuration = Configuration.GetSection("ApplicationConfiguration")
                .Get<ApplicationConfiguration>();

            app.UseCors(builder =>
                builder
                .WithOrigins(configuration.MessageStoreApiUrl)
                .AllowAnyHeader()
                );

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
