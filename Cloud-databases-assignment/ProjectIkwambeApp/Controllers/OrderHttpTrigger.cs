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

        [Function(nameof(OrderHttpTrigger.AddOrder))]
        public async Task<HttpResponseData> AddOrder([HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "orders")] HttpRequestData req, FunctionContext executionContext)
        {
            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            OrderDTO orderDTO = JsonConvert.DeserializeObject<OrderDTO>(requestBody);
            HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(await _orderService.AddOrder(orderDTO));
            return response;
           
        }

        [Function(nameof(OrderHttpTrigger.GetOrders))]
        public async Task<HttpResponseData> GetOrders([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "orders")] HttpRequestData req, FunctionContext executionContext)
        {
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                await response.WriteAsJsonAsync(await _orderService.GetAllOrders());

                return response;
            }
        }

        [Function(nameof(OrderHttpTrigger.GetOrdersById))]
        public async Task<HttpResponseData> GetOrdersById([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "orders/{orderId}")] HttpRequestData req, string orderId, FunctionContext executionContext)
        {

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(await _orderService.GetOrderById(orderId));
            return response;

        }

        [Function(nameof(OrderHttpTrigger.UpdateOrderStatus))]
        public async Task<HttpResponseData> UpdateOrderStatus([HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "orders/{orderId}")] HttpRequestData req, string orderId, FunctionContext executionContext)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            OrderStatusDTO orderDTO = JsonConvert.DeserializeObject<OrderStatusDTO>(requestBody);
            HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(await _orderService.UpdateOrderStatus(orderDTO, orderId));
            return response;
        }

        [Function(nameof(OrderHttpTrigger.DeleteOrder))]
        public async Task<HttpResponseData> DeleteOrder([HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "users/{userId}")] HttpRequestData req, string orderId, FunctionContext executionContext)
        {
            
            HttpResponseData response = req.CreateResponse(HttpStatusCode.Accepted);
            await _orderService.DeleteOrderAsync(orderId);
            await response.WriteStringAsync("The order has been deleted");
            return response;
        }
    }
}
