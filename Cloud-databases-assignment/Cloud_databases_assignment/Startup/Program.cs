using Cloud_databases_assignment.ErrorHandlerMiddleware;
using Domain;
using DAL.DBContext;
using DAL.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Orders;
using Infrastructure.Services.Products;
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
				.ConfigureFunctionsWorkerDefaults((IFunctionsWorkerApplicationBuilder Builder) => {
					Builder.UseMiddleware<GlobalErrorHandler>();
				})
				.ConfigureServices(Configure)
				.Build();

			host.Run();
		}

		//Dependency Injection for repos,services and db context
		static void Configure(HostBuilderContext Builder, IServiceCollection Services) {

			// DBContext
			Services.AddDbContext<CosmosDbContext>(option =>
			{
				option.UseCosmos(Environment.GetEnvironmentVariable("CosmosDb:Account", EnvironmentVariableTarget.Process), Environment.GetEnvironmentVariable("CosmosDb:Key", EnvironmentVariableTarget.Process), Environment.GetEnvironmentVariable("CosmosDb:DatabaseName", EnvironmentVariableTarget.Process));

			});

			// Repositories
			Services.AddTransient(typeof(ICosmosReadRepository<>), typeof(CosmosReadRepository<>));
			Services.AddTransient(typeof(ICosmosWriteRepository<>), typeof(CosmosWriteRepository<>));


			// Services
			Services.AddScoped<IOrderService, OrderService>();
			Services.AddScoped<IProductService, ProductService>();
			Services.AddScoped<IUserService, UserService>();
			Services.AddScoped<IProductReviewService, ProductReviewService>();


		}
	}
}


