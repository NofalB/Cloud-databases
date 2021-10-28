using Domain;
using Domain.DTO;
using HttpMultipartParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Products
{
    public interface IProductReviewService
    {
        Task<IEnumerable<ProductReview>> GetAllProductReviews();

        Task<ProductReview> GetProductReviewById(string productReviewId);

        Task<ProductReview> AddProductReview(ProductReviewDTO productReviewDTO);

        Task<ProductReview> UpdateProductReview(ProductReviewDTO productReviewDTO, string productReviewId);
    }
}
