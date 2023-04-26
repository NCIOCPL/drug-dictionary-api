using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NCI.OCPL.Api.Common;
using NCI.OCPL.Api.DrugDictionary.Models;
using NCI.OCPL.Api.DrugDictionary.Services;

namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Defines the configuration for the Drug Dictionary API
    /// </summary>
    public class Startup : NciStartupBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NCI.OCPL.Api.DrugDictionary.Startup"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public Startup(IConfiguration configuration)
            : base(configuration) { }


        /*****************************
         * ConfigureServices methods *
         *****************************/

        /// <summary>
        /// Adds the configuration mappings.
        /// </summary>
        /// <param name="services">Services.</param>
        protected override void AddAdditionalConfigurationMappings(IServiceCollection services)
        {
        }

        /// <summary>
        /// Adds in the application specific services
        /// </summary>
        /// <param name="services">Services.</param>
        protected override void AddAppServices(IServiceCollection services)
        {
            // Add our Query Services
            services.AddTransient<IAutosuggestQueryService, ESAutosuggestQueryService>();
            services.AddTransient<IDrugsQueryService, ESDrugsQueryService>();

            services.Configure<DrugDictionaryAPIOptions>(Configuration.GetSection("DrugDictionaryAPI"));

            // Get name for the Healthcheck alias.
            services.AddTransient<IESAliasNameProvider>(p =>
            {
                string alias = Configuration["DrugDictionaryAPI:AliasName"];
                return new ESAliasNameProvider() { Name = alias };
            });
        }

        /*****************************
         *     Configure methods     *
         *****************************/

        /// <summary>
        /// Configure the specified app and env.
        /// </summary>
        /// <returns>The configure.</returns>
        /// <param name="app">App.</param>
        /// <param name="env">Env.</param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        protected override void ConfigureAppSpecific(IApplicationBuilder app, IWebHostEnvironment env)
        {
            return;
        }
    }
}
