namespace WhatIsHeDoing.DomainModels.APITest
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        public static void Main(string[] args) => BuildWebHost(args).Run();

        public static IWebHost BuildWebHost(string[] args) => WebHost
            .CreateDefaultBuilder(args)
            .UseKestrel(opt => opt.AddServerHeader = false)
            .UseStartup<Startup>()
            .Build();
    }
}
