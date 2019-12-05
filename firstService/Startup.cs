using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Datadog.Trace;
using Datadog.Trace.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace firstService
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
            services.AddOptions<RemoteServiceSettings>().Configure(o => Configuration.GetSection("Remote").Bind(o)).ValidateDataAnnotations();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            SetupDatadogTracing();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void SetupDatadogTracing()
        {
            // read default configuration sources (env vars, web.config, datadog.json)
            TracerSettings settings = TracerSettings.FromDefaultSources();

            settings.LogsInjectionEnabled = true;

            // create a new Tracer using these settings
            Tracer tracer = new Tracer(settings);
            // set the global tracer
            Tracer.Instance = tracer;
        }
    }

    public class RemoteServiceSettings
    {
        [Required]
        public string ConnectionString {get; set;} = "";

        public bool UseRestSharp {get;set;} = false;
    }
}
