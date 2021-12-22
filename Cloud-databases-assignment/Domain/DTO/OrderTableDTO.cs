using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class OrderTableDTO
    {
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime ShippingDate { get; set; }

        public OrderTableDTO()
        {

        }
    }
}
