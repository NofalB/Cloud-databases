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
        public Guid OrderId { get; set; }

        [JsonRequired]
        public Guid ProductId { get; set; }

        [JsonRequired]
        public int Quantity { get; set; }

        [JsonRequired]
        public double TotalPrice { get; set; }

        [JsonRequired]
        public DateTime OrderDate { get; set; }

        [JsonRequired]
        public OrderStatus OrderStatus { get; set; }

        public string PartitionKey { get; set; }

        public OrderDTO(int quantity, double totalPrice)
        {
            Quantity = quantity;
            TotalPrice = totalPrice;

        }
    }
}
