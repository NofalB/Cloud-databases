using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ProductDTO
    {
        [JsonRequired]
        public string ProductName { get; set; }

        [JsonRequired]
        public ProductType ProductType { get; set; }

        [JsonRequired]
        public string Description { get; set; }

        [JsonRequired]
        public int Quantity { get; set; }

        [JsonRequired]
        public double Price { get; set; }

        [JsonRequired]
        public List<ProductImage> ImageURLs { get; set; }

        public ProductDTO()
        {

        }
    }
}
