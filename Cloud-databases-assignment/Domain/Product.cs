using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Domain
{
    public class Product
    {
        [JsonProperty("id")]
        public Guid ProductId { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("productType")]
        public ProductType ProductType { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("imageUrls")]
        public List<ProductImage> ImageURLs { get; set; }

        public string PartitionKey { get; set; }

        public Product()
        {
            
        }
    }
}
