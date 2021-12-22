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

        //queue trigger to process and store orders
        [Function("OrdersQueue")]
        public async Task OrdersQueue([QueueTrigger("order-queue")] string order,
            FunctionContext context)
        {
            var logger = context.GetLogger("OrdersQueue");
            logger.LogInformation($"Processing Order: {order}");

            var orderDTO = JsonConvert.DeserializeObject<OrderDTO>(order);

            await _orderService.AddOrder(orderDTO);
        }

        [Function(nameof(OrderHttpTrigger.AddOrder))]
        public async Task<QueueOrderOutput> AddOrder([HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "orders")] HttpRequestData req, FunctionContext executionContext)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var orderDTO = JsonConvert.DeserializeObject<OrderDTO>(requestBody);
            var response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(orderDTO);

            return new QueueOrderOutput()
            {
                Orderdata = requestBody,
                HttpResponse = response
            };


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

        [Function(nameof(OrderHttpTrigger.UpdateOrder))]
        public async Task<HttpResponseData> UpdateOrder([HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "orders/{orderId}")] HttpRequestData req, string orderId, FunctionContext executionContext)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            OrderUpdateDTO orderDTO = JsonConvert.DeserializeObject<OrderUpdateDTO>(requestBody);
            HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(await _orderService.UpdateOrder(orderDTO, orderId));
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

        //class to store message in queue
        public class QueueOrderOutput
        {
            [QueueOutput("order-queue", Connection = "BlobCredentialOptions:ConnectionString")]
            public string Orderdata { get; set; }
            public HttpResponseData HttpResponse { get; set; }
        }
    }
}
