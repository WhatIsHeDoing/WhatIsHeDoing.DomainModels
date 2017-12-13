namespace WhatIsHeDoing.DomainModels.APITest
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.AspNetCore.Swagger;
    using System;

    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.InputFormatters.Add(new XmlSerializerInputFormatter());
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });

            services.AddSwaggerGen(config =>
            {
                config.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory +
                    @"WhatIsHeDoing.DomainModels.APITest.xml");

                config.SwaggerDoc("v1", new Info
                {
                    Contact = new Contact
                    {
                        Name = "WhatIsHeDoing",
                        Url = "https://www.nuget.org/packages/WhatIsHeDoing.DomainModels/"
                    },
                    Description = "Tests the use of WhatIsHeDoing.DomainModels in Web API.",
                    License = new License
                    {
                        Name = "Unlicense",
                        Url = "https://unlicense.org/"
                    },
                    Title = "WhatIsHeDoing.DomainModels",
                    Version = "v1",
                });
            });
        }

        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();

            app.UseSwaggerUI(config =>
            {
                config.ShowJsonEditor();
                config.ShowRequestHeaders();

                config.SwaggerEndpoint(
                    "/swagger/v1/swagger.json", "WhatIsHeDoing.DomainModels");
            });
        }
    }
}
