namespace WhatIsHeDoing.DomainModels.APITest
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using System;

    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(config => config.EnableEndpointRouting = false)
                .AddXmlSerializerFormatters();

            services.AddSwaggerGen(config =>
            {
                config.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory +
                    @"WhatIsHeDoing.DomainModels.APITest.xml");

                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Contact = new OpenApiContact
                    {
                        Name = "WhatIsHeDoing",
                        Url = new Uri("https://www.nuget.org/packages/WhatIsHeDoing.DomainModels/")
                    },
                    Description = "Tests the use of WhatIsHeDoing.DomainModels in Web API.",
                    License = new OpenApiLicense
                    {
                        Name = "Unlicense",
                        Url = new Uri("https://unlicense.org/")
                    },
                    Title = "WhatIsHeDoing.DomainModels",
                    Version = "v1",
                });
            });
        }

        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint(
                    "/swagger/v1/swagger.json", "WhatIsHeDoing.DomainModels");
            });
        }
    }
}
