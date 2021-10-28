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

        public ProductReviewService(ICosmosReadRepository<ProductReview> productReviewReadRepository, ICosmosWriteRepository<ProductReview> productReviewWriteRepository)
        {
            _productReviewReadRepository = productReviewReadRepository;
            _productReviewWriteRepository = productReviewWriteRepository;
        }

        public async Task<ProductReview> AddProductReview(ProductReviewDTO productReviewDTO)
        {
            Guid id = Guid.NewGuid();
            ProductReview productReview = new ProductReview();
            productReview.ReviewId = id;
            productReview.ProductId = productReviewDTO.ProductId;
            productReview.Title = productReviewDTO.Title;
            productReview.Description = productReviewDTO.Description;
            productReview.PublishDate = DateTime.Now;
            productReview.LastEditDate = DateTime.Now;
            productReview.Rating = productReviewDTO.Rating;
            productReview.PartitionKey = id.ToString();

            return await _productReviewWriteRepository.AddAsync(productReview);
        }

        public async Task<IEnumerable<ProductReview>> GetAllProductReviews()
        {
            return await _productReviewReadRepository.GetAll().ToListAsync();
        }

        public async Task<ProductReview> GetProductReviewById(string productReviewId)
        {
            var productReviewGuid = Guid.Parse(productReviewId);
            var productReview = await _productReviewReadRepository.GetAll().FirstOrDefaultAsync(t => t.ReviewId == productReviewGuid);
            return productReview;
        }

        public async Task<ProductReview> UpdateProductReview(ProductReviewDTO productReviewDTO, string productReviewId)
        {
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
