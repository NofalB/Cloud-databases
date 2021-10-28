using Domain;
using Domain.DTO;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
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

        public ProductService(ICosmosReadRepository<Product> productReadRepository, ICosmosWriteRepository<Product> productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        public async Task<Product> AddProduct(ProductDTO productDto)
        {
            Product product = new Product();
            product.ProductId = new Guid();
            product.ProductName = productDto.ProductName;
            product.ProductType = productDto.ProductType;
            product.Description = productDto.Description;
            product.Price = productDto.Quantity;
            product.Price = productDto.Price;
            product.ImageUrl = "";

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
            var productGuid = Guid.Parse(productId);
            var product = await _productReadRepository.GetAll().FirstOrDefaultAsync(t => t.ProductId == productGuid);
            return product;
        }

        public async Task<Product> UpdateProduct(ProductDTO productDto, string productId)
        {
            var existingProduct = await GetProductById(productId);
            if (existingProduct != null)
            {
                Product product = new Product();
                product.ProductId = existingProduct.ProductId;
                product.ProductName = productDto.ProductName;
                product.ProductType = productDto.ProductType;
                product.Description = productDto.Description;
                product.Price = productDto.Quantity;
                product.Price = productDto.Price;
                product.ImageUrl = existingProduct.ImageUrl;

                return await _productWriteRepository.Update(existingProduct);
            }
            else
            {
                throw new InvalidOperationException("The product ID provided does not exist");
            }
        }
    }
}
