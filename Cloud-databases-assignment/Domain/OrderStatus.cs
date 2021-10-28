using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum  OrderStatus
    {
        [EnumMember(Value = "ordered")]
        ordered,
        [EnumMember(Value = "delivered")]
        delivered,
        [EnumMember(Value = "processing")]
        processing
    }
}
