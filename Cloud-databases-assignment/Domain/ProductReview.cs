using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ProductReview
    {
        [JsonProperty("id")]
        public Guid ReviewId { get; set; }

        [JsonProperty("productId")]
        public Guid ProductId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("productRating")]
        public ProductRating Rating { get; set; }

        [JsonProperty("publishDate")]
        public DateTime PublishDate { get; set; }

        [JsonProperty("lastEditDate")]
        public DateTime LastEditDate { get; set; }
        public string PartitionKey { get; set; }

    }
}
