using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Order
    {
        [JsonProperty("id")]
        public Guid OrderId { get; set; }

        [JsonProperty("productId")]
        public Guid ProductId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("totalPrice")]
        public double TotalPrice { get; set; }

        [JsonProperty("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonProperty("orderStatus")]
        public OrderStatus OrderStatus { get; set; }

        [JsonProperty("shippingDate")]
        public DateTime ShippingDate { get; set; }

        [JsonProperty("userId")]
        public Guid? UserId { get; set; }

        public string PartitionKey { get; set; }

        public Order(Guid orderId, Guid productId, int quantity, double totalPrice, OrderStatus orderStatus, DateTime shippingDate, Guid? userId)
        {
            OrderId = orderId;
            ProductId = productId;
            OrderId = orderId;
            Quantity = quantity;
            TotalPrice = totalPrice;
            OrderDate = DateTime.Now;
            OrderStatus = orderStatus;
            ShippingDate = shippingDate;
            UserId = userId;
            PartitionKey = orderId.ToString();

        }

        public Order()
        {

        }
    }
}
