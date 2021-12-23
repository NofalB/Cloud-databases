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

        [JsonRequired]
        public DateTime ShippingDate { get; set; }

        //setting it nullable so it doesnt always have to filled for the order
        public Guid? UserId { get; set; }

        public OrderDTO(Guid productId,int quantity, double totalPrice, DateTime shippingDate, Guid? userId)
        {
            ProductId = productId;
            Quantity = quantity;
            TotalPrice = totalPrice;
            ShippingDate = shippingDate;
            UserId = userId;

        }
    }
}
