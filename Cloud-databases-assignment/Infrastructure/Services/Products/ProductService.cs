using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain;
using Domain.DTO;
using HttpMultipartParser;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly ICosmosReadRepository<Product> _productReadRepository;
        private readonly ICosmosWriteRepository<Product> _productWriteRepository;
        private BlobServiceClient blobServiceClient;
        private BlobContainerClient containerClient;


        public ProductService(ICosmosReadRepository<Product> productReadRepository, ICosmosWriteRepository<Product> productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;

            blobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("BlobCredentialOptions:ConnectionString", EnvironmentVariableTarget.Process));
            containerClient = blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("BlobCredentialOptions:ContainerName", EnvironmentVariableTarget.Process));
        }

        public async Task<Product> AddProduct(ProductDTO productDto)
        {
            if (productDto == null)
            {
                throw new NullReferenceException($"{nameof(productDto)} cannot be null.");
            }

            Guid id = Guid.NewGuid();
            Product product = new Product();
            product.ProductId = id;
            product.ProductName = productDto.ProductName;
            product.ProductType = productDto.ProductType;
            product.Description = productDto.Description;
            product.Quantity = productDto.Quantity;
            product.Price = productDto.Price;
            product.ImageUrl = "";
            product.PartitionKey = id.ToString();

            return await _productWriteRepository.AddAsync(product);

        }

        public async Task DeleteProductAsync(string productId)
        {
            var id = !string.IsNullOrEmpty(productId) ? productId : throw new ArgumentNullException($"{productId} cannot be null or empty string.");
            Product product = await GetProductById(id);

            if (product != null)
            {
                await _productWriteRepository.Delete(product);
            }
            else
            {
                throw new InvalidOperationException($"The product ID {productId} provided is invalid.");
            }
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _productReadRepository.GetAll().ToListAsync();
        }

        public async Task<Product> GetProductById(string productId)
        {
            try
            {
                Guid id = !string.IsNullOrEmpty(productId) ? Guid.Parse(productId) : throw new ArgumentNullException("No productId was provided.");

                var product = await _productReadRepository.GetAll().FirstOrDefaultAsync(t => t.ProductId == id);
                return product;
            }
            catch
            {
                throw new InvalidOperationException($"Invalid productReviewId {productId} provided.");
            }            
        }

        public async Task<Product> UpdateProduct(ProductDTO productDto, string productId)
        {
            if (productDto == null)
            {
                throw new NullReferenceException($"{nameof(productDto)} cannot be null.");
            }

            var existingProduct = await GetProductById(productId);
            if (existingProduct != null)
            {
                existingProduct.ProductName = productDto.ProductName;
                existingProduct.ProductType = productDto.ProductType;
                existingProduct.Description = productDto.Description;
                existingProduct.Quantity = productDto.Quantity;
                existingProduct.Price = productDto.Price;

                return await _productWriteRepository.Update(existingProduct);
            }
            else
            {
                throw new InvalidOperationException("The product ID provided does not exist");
            }
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            return await _productWriteRepository.Update(product);
        }

        public async Task UploadImage(string productId, FilePart file)
        {
            if (file.ContentType == "image/jpeg" || file.ContentType == "image/bmp" || file.ContentType == "image/png")
            {
                // Get a reference to a blob
                BlobClient blobClient = containerClient.GetBlobClient(file.Name);

                // Upload the file
                await blobClient.UploadAsync(file.Data, new BlobHttpHeaders { ContentType = file.ContentType });

                //get the URL of the uploaded image
                var blobUrl = blobClient.Uri.AbsoluteUri;

                var product = await GetProductById(productId);

                //set the new url for the existing story
                product.ImageUrl = blobUrl;

                await UpdateProduct(product);
            }
            else
            {
                throw new InvalidOperationException("Invalid content type. Media type not supported. Upload a valid image of type jpeg,bmp or png.");
            }

        }
    }
}
