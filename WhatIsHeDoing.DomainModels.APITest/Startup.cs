using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WhatIsHeDoing.DomainModels.APITest
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) => services
            .AddMvc(options =>
            {
                options.InputFormatters.Add(new XmlSerializerInputFormatter());
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });

        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
