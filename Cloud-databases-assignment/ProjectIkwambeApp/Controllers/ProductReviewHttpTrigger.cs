using Domain.DTO;
using Infrastructure.Services.Products;
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
    public class ProductReviewHttpTrigger
    {
        ILogger Logger { get; }
        private readonly IProductReviewService _productReviewService;

        public ProductReviewHttpTrigger(ILogger<ProductReviewHttpTrigger> Logger, IProductReviewService productReview)
        {
            this.Logger = Logger;
            _productReviewService = productReview;
        }

        [Function(nameof(ProductReviewHttpTrigger.AddReview))]
        public async Task<HttpResponseData> AddReview([HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "product/reviews")] HttpRequestData req, FunctionContext executionContext)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ProductReviewDTO productReviewDTO = JsonConvert.DeserializeObject<ProductReviewDTO>(requestBody);
            HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(await _productReviewService.AddProductReview(productReviewDTO));
            return response;

        }

        [Function(nameof(ProductReviewHttpTrigger.GetUsers))]
        public async Task<HttpResponseData> GetUsers([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "product/reviews")] HttpRequestData req, FunctionContext executionContext)
        {
            
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(await _productReviewService.GetAllProductReviews());

            return response;
            
        }

        [Function(nameof(ProductReviewHttpTrigger.GetUsersById))]
        public async Task<HttpResponseData> GetUsersById([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "product/reviews/{reviewId}")] HttpRequestData req, string reviewId, FunctionContext executionContext)
        {

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(await _productReviewService.GetProductReviewById(reviewId));
            return response;

        }

        [Function(nameof(ProductReviewHttpTrigger.UpdateUser))]
        public async Task<HttpResponseData> UpdateUser([HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "users/{reviewId}")] HttpRequestData req, string reviewId, FunctionContext executionContext)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ProductReviewDTO productReviewDTO = JsonConvert.DeserializeObject<ProductReviewDTO>(requestBody);
            HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(await _productReviewService.UpdateProductReview(productReviewDTO, reviewId));
            return response;
        }
    }
}
