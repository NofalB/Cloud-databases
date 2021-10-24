using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class OrderDTO
    {
        [JsonRequired]
        public Guid ProductId { get; set; }

        [JsonRequired]
        public int Quantity { get; set; }

        [JsonRequired]
        public double TotalPrice { get; set; }

        public OrderDTO(Guid productId,int quantity, double totalPrice)
        {
            ProductId = productId;
            Quantity = quantity;
            TotalPrice = totalPrice;

        }
    }
}
