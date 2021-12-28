using Assessment1.BAL.Business;
using Assessment1.BAL.IBusiness;
using Assessment1.ConcreteClass;
using Assessment1.DAL.IRepository;
using Assessment1.DAL.Repository;
using Assessment1.Data;
using Assessment1.Filters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment1
{
    public class Startup
    {
        Common common = new Common();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddDbContext<ApplicationDBContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            options.UseSqlServer(getConnectionStringFromSecret()));

            services.AddScoped<IBatchRepository, BatchRepository>();
            services.AddScoped<IBatchBusiness, BatchBusiness>();

            services.AddControllers().ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });
            services.AddControllers();
            services.AddApplicationInsightsTelemetry();
            services.AddMvc(options =>
            {
                options.Filters.Add(new ValidationFilter());
            }).AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=WeatherForecast}/{action=Get}/{id?}");
            //});
        }

        private string getConnectionStringFromSecret()
        {
            string url = Configuration.GetValue<string>("KeyVault:url");
            string clientId = Configuration.GetValue<string>("KeyVault:ClientId");
            string clientSecret = Configuration.GetValue<string>("KeyVault:ClientSecret");
            var _keyVaultClient = new KeyVaultClient(
            async (string authority, string resource, string scope) =>
            {
                var authContext = new AuthenticationContext(authority);
                //var clientCred = new ClientCredential("73f718ec-faa9-4b40-bc9b-0854988b3cdd", "~Ye7Q~.bP~fMWe5f3EsD8-mISKoByzbvTrhoj");
                var clientCred = new ClientCredential(clientId, clientSecret);
                var result = await authContext.AcquireTokenAsync(resource, clientCred);
                return result.AccessToken;
            });

            //_keyVaultClient.SetSecretAsync(url, "Password", "This is my password");

            var result = _keyVaultClient.GetSecretAsync(url, "SecretKey-AssesmentOne-ConnectionString").GetAwaiter().GetResult();
            return result.Value;
        }
    }
}
