using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RealtorFirm.BLL.Interfaces;
using RealtorFirm.BLL.Models;
using RealtorFirm.BLL.Services;
using RealtorFirm.DAL.Interfaces;
using RealtorFirm.DAL.Repositories;
using RealtorFirm.DAL.Entities;
using System.IO;

namespace RealtorFirm.PL
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var menu = host.Services.GetRequiredService<AppMenu>();
            menu.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "db");
                    if (!Directory.Exists(dbPath)) Directory.CreateDirectory(dbPath);

                    services.AddSingleton<IRepository<Client>>(
                        new FileRepository<Client>(Path.Combine(dbPath, "clients.json")));

                    services.AddSingleton<IRepository<RealEstate>>(
                        new FileRepository<RealEstate>(Path.Combine(dbPath, "realestate.json")));

                    services.AddTransient<IClientService, ClientService>();
                    services.AddTransient<IRealEstateService, RealEstateService>();
                    services.AddTransient<IOfferService, OfferService>();

                    services.AddTransient<AppMenu>();
                });
    }
}
    

