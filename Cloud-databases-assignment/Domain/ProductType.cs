using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public enum ProductType
    {
        [EnumMember(Value = "computer")]
        computer,
        [EnumMember(Value = "kitchen")]
        kitchen,
        [EnumMember(Value = "tv")]
        tv,
        [EnumMember(Value = "sports")]
        sports,
        [EnumMember(Value = "audio")]
        audio,
        [EnumMember(Value = "gaming")]
        gaming,
        [EnumMember(Value = "film")]
        film,
        [EnumMember(Value = "music")]
        music
    }
}
