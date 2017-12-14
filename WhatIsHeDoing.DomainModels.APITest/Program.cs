namespace WhatIsHeDoing.DomainModels.APITest
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public class Program
    {
        public static void Main(string[] args) => BuildWebHost(args).Run();

        public static IWebHost BuildWebHost(string[] args) => WebHost
            .CreateDefaultBuilder(args)
            .UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build())
            .UseKestrel(opt => opt.AddServerHeader = false)
            .UseStartup<Startup>()
            .Build();
    }
}
