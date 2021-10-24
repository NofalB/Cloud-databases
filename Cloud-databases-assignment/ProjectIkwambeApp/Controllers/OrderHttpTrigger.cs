using Domain.DTO;
using Infrastructure.Services.Orders;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_databases_assignment.Controllers
{
    public class OrderHttpTrigger
    {
        ILogger Logger { get; }
        private readonly IOrderService _orderService;

        public OrderHttpTrigger(ILogger<OrderHttpTrigger> Logger, IOrderService orderService)
        {
            this.Logger = Logger;
            _orderService = orderService;
        }

        [Function("AddOrder")]
        public async Task<HttpResponseData> AddOrder([HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "orders")] HttpRequestData req, FunctionContext executionContext)
        {
            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            OrderDTO orderDTO = JsonConvert.DeserializeObject<OrderDTO>(requestBody);
            HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(await _orderService.AddOrder(orderDTO));
            return response;
           
        }
    }
}
