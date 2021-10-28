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
    public class ProductReviewService : IProductReviewService
    {
        private readonly ICosmosReadRepository<ProductReview> _productReviewReadRepository;
        private readonly ICosmosWriteRepository<ProductReview> _productReviewWriteRepository;
        private readonly IProductService _productService;

        public ProductReviewService(ICosmosReadRepository<ProductReview> productReviewReadRepository, ICosmosWriteRepository<ProductReview> productReviewWriteRepository, IProductService productService)
        {
            _productReviewReadRepository = productReviewReadRepository;
            _productReviewWriteRepository = productReviewWriteRepository;
            _productService = productService;
        }

        public async Task<ProductReview> AddProductReview(ProductReviewDTO productReviewDTO)
        {
            try
            {
                if (productReviewDTO == null)
                {
                    throw new NullReferenceException($"{nameof(productReviewDTO)} cannot be null.");
                }
                var product = await _productService.GetProductById(productReviewDTO.ProductId.ToString());

                if (product == null)
                {
                    throw new NullReferenceException($"this product with product id {productReviewDTO.ProductId} does not exist");

                }
                Guid id = Guid.NewGuid();
                ProductReview productReview = new ProductReview();
                productReview.ProductReviewId = id;
                productReview.ProductId = productReviewDTO.ProductId;
                productReview.Title = productReviewDTO.Title;
                productReview.Description = productReviewDTO.Description;
                productReview.PublishDate = DateTime.Now;
                productReview.LastEditDate = DateTime.Now;
                productReview.Rating = productReviewDTO.Rating;
                productReview.PartitionKey = id.ToString();

                return await _productReviewWriteRepository.AddAsync(productReview);

            }
            catch
            {
                throw new InvalidOperationException("Please check all the fields are filled");
            }

            
        }

        public async Task<IEnumerable<ProductReview>> GetAllProductReviews()
        {
            return await _productReviewReadRepository.GetAll().ToListAsync();
        }

        public async Task<ProductReview> GetProductReviewById(string productReviewId)
        {
            try
            {
                Guid id = !string.IsNullOrEmpty(productReviewId) ? Guid.Parse(productReviewId) : throw new ArgumentNullException("No productReviewId was provided.");

                var productReview = await _productReviewReadRepository.GetAll().FirstOrDefaultAsync(t => t.ProductReviewId == id);
                return productReview;
            }
            catch
            {
                throw new InvalidOperationException($"Invalid productReviewId {productReviewId} provided.");
            }           
        }

        public async Task<ProductReview> UpdateProductReview(ProductReviewDTO productReviewDTO, string productReviewId)
        {
            if (productReviewDTO == null)
            {
                throw new NullReferenceException($"{nameof(productReviewDTO)} cannot be null.");
            }

            var existingProductReview = await GetProductReviewById(productReviewId);
            if (existingProductReview != null)
            {
                existingProductReview.ProductId = productReviewDTO.ProductId;
                existingProductReview.Title = productReviewDTO.Title;
                existingProductReview.Description = productReviewDTO.Description;
                existingProductReview.LastEditDate = DateTime.Now;
                existingProductReview.Rating = productReviewDTO.Rating;

                return await _productReviewWriteRepository.Update(existingProductReview);
            }
            else
            {
                throw new InvalidOperationException("The product review ID provided does not exist");
            }
        }
    }
}
