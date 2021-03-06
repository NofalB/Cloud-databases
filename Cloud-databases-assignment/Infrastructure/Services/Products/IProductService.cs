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
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();

        Task<Product> GetProductById(string productId);

        Task<Product> AddProduct(ProductDTO product);

        Task<Product> UpdateProduct(ProductDTO product, string productId);

        Task<Product> UpdateProduct(Product product);

        Task DeleteProductAsync(string productId);

        Task UploadImage(string productId, FilePart file);

    }
}
