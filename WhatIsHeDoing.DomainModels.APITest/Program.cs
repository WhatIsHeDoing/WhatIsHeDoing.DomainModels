using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

builder.Services
    .AddControllers()
    .AddXmlSerializerFormatters();

builder.Services.AddSwaggerGen(config =>
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

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(config =>
{
    config.SwaggerEndpoint(
        "/swagger/v1/swagger.json", "WhatIsHeDoing.DomainModels");

    config.RoutePrefix = string.Empty;
});

app.MapControllers();
app.Run();
