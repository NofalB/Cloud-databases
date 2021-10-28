using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Domain.DTO;
using HttpMultipartParser;
using Infrastructure.Services.Products;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Cloud_databases_assignment.Controllers
{
    public class ProductHttpTrigger
    {
        ILogger Logger { get; }
        private readonly IProductService _productService;

        public ProductHttpTrigger(ILogger<ProductHttpTrigger> Logger, IProductService productService)
        {
            this.Logger = Logger;
            _productService = productService;
        }

        [Function(nameof(ProductHttpTrigger.AddProduct))]
        public async Task<HttpResponseData> AddProduct([HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "products")] HttpRequestData req, FunctionContext executionContext)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ProductDTO productDTO = JsonConvert.DeserializeObject<ProductDTO>(requestBody);
            HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(await _productService.AddProduct(productDTO));
            return response;

        }

        [Function(nameof(ProductHttpTrigger.GetProducts))]
        public async Task<HttpResponseData> GetProducts([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "products")] HttpRequestData req, FunctionContext executionContext)
        {
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                await response.WriteAsJsonAsync(await _productService.GetAllProducts());

                return response;
            }
        }

        [Function(nameof(ProductHttpTrigger.GetProductsById))]
        public async Task<HttpResponseData> GetProductsById([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "products/{productId}")] HttpRequestData req, string productId, FunctionContext executionContext)
        {

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(await _productService.GetProductById(productId));
            return response;

        }

        [Function(nameof(ProductHttpTrigger.UpdateProduct))]
        public async Task<HttpResponseData> UpdateProduct([HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "products/{productId}")] HttpRequestData req, string productId, FunctionContext executionContext)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ProductDTO productDTO = JsonConvert.DeserializeObject<ProductDTO>(requestBody);
            HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(await _productService.UpdateProduct(productDTO, productId));
            return response;
        }

        [Function(nameof(ProductHttpTrigger.UploadProductImage))]
        public async Task<HttpResponseData> UploadProductImage([HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "upload/{productId}")] HttpRequestData req, string productId, FunctionContext executionContext)
        {      
            var parsedFormBody = MultipartFormDataParser.ParseAsync(req.Body);
            var file = parsedFormBody.Result.Files[0];
            HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
            await _productService.UploadImage(productId, file);
            await response.WriteStringAsync("Uploaded image file");
            return response;
        }

        [Function(nameof(ProductHttpTrigger.DeleteProduct))]
        public async Task<HttpResponseData> DeleteProduct([HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "products/{productId}")] HttpRequestData req, string productId, FunctionContext executionContext)
        {

            HttpResponseData response = req.CreateResponse(HttpStatusCode.Accepted);
            await _productService.DeleteProductAsync(productId);
            await response.WriteStringAsync($"The specified product with id {productId} has been deleted");
            return response;
        }
    }
}
