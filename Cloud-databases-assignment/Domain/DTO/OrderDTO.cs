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
        public int Quantity { get; set; }

        [JsonRequired]
        public double TotalPrice { get; set; }

        public OrderDTO(int quantity, double totalPrice)
        {
            Quantity = quantity;
            TotalPrice = totalPrice;

        }
    }
}
