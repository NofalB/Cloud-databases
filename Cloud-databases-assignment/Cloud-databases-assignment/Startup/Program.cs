using Infrastructure.DBContext;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;


namespace Cloud_databases_assignment.Startup
{
	public class Program
	{
		public static void Main()
		{
			IHost host = new HostBuilder()
				.ConfigureServices(Configure)
				.Build();

			host.Run();
		}

		static void Configure(HostBuilderContext Builder, IServiceCollection Services)
		{

            // DBContext
            Services.AddDbContext<CosmosDbContext>(option =>
            {
                option.UseCosmos("https://projectikwambedb.documents.azure.com:443/", "0gHgOaqhe8NAjY0b02DurzqSZHiKI5NF9zQsRkAhqJsJmOIcPylMGZR44ZzmLSrbkhztzQeW8AKfu7BJnZ2nYQ==", "ProjectIkwambeDB");

            });
        }
	}
}