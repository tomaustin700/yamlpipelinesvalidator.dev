using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YAMLPipelineValidator.Classes
{
    public class ErrorResponse
    {
        //[JsonProperty("$id")]
        //[JsonConverter(typeof(ParseStringConverter))]
        //public long Id { get; set; }

        [JsonProperty("innerException")]
        public object InnerException { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("typeName")]
        public string TypeName { get; set; }

        [JsonProperty("typeKey")]
        public string TypeKey { get; set; }

        [JsonProperty("errorCode")]
        public long ErrorCode { get; set; }

        [JsonProperty("eventId")]
        public long EventId { get; set; }
    }
}
