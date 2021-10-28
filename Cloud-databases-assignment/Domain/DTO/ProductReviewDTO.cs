using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ProductReviewDTO
    {
        [JsonRequired]
        public Guid ProductId { get; set; }

        [JsonRequired]
        public string Title { get; set; }

        [JsonRequired]
        public string Description { get; set; }

        [JsonRequired]
        public ProductRating Rating { get; set; }

    }
}
