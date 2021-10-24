using Domain;
using Infrastructure.DBContext;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Orders;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Functions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;

namespace Cloud_databases_assignment.Startup {
	public class Program {
		public static void Main() {
			IHost host = new HostBuilder()
				.ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
				.ConfigureOpenApi()
				.ConfigureServices(Configure)
				.Build();

			host.Run();
		}

		static void Configure(HostBuilderContext Builder, IServiceCollection Services) {

			// DBContext
			Services.AddDbContext<CosmosDbContext>(option =>
			{
				option.UseCosmos("https://localhost:8081", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", "WidgetCoDB");

			});

			// Repositories
			Services.AddTransient(typeof(ICosmosReadRepository<>), typeof(CosmosReadRepository<>));
			Services.AddTransient(typeof(ICosmosWriteRepository<>), typeof(CosmosWriteRepository<>));


			// Services
			Services.AddScoped<IOrderService, OrderService>();
		}

		public static string GetEnvironmentVariable(string name)
		{
			return name + ": " +
				System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
		}
	}
}

