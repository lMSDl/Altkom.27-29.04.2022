//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    [Flags]
    //[JsonConverter(typeof(StringEnumConverter))]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Roles
    {
        Read = 1 << 0,
        Write = 1 << 1,
        Delete = 1 << 2,
        Update = 1 << 3
    }
}
