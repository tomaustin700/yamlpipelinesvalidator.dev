using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API.Classes
{
    public class ErrorResponse
    {
        [JsonPropertyName("innerException")]
        public object InnerException { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("typeName")]
        public string TypeName { get; set; }

        [JsonPropertyName("typeKey")]
        public string TypeKey { get; set; }

        [JsonPropertyName("errorCode")]
        public long ErrorCode { get; set; }

        [JsonPropertyName("eventId")]
        public long EventId { get; set; }
    }
}
