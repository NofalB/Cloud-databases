using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class OrderStatusDTO
    {
        [JsonRequired]
        public string OrderStatus { get; set; }
    }
}
