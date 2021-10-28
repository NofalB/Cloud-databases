using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class OrderUpdateDTO
    {
        [JsonRequired]
        public OrderStatus OrderStatus { get; set; }

        [JsonRequired]
        public DateTime ShippingDate { get; set; }
    }
}
