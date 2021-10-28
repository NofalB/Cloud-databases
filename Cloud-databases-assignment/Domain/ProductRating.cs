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
    public enum ProductRating
    {
        [EnumMember(Value = "excellent")]
        excellent,
        [EnumMember(Value = "good")]
        good,
        [EnumMember(Value = "okay")]
        okay,
        [EnumMember(Value = "bad")]
        bad,
        [EnumMember(Value = "verybad")]
        verybad,

    }
}
