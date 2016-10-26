using System.Collections.Generic;
using Newtonsoft.Json;

namespace AwesomeAPI.Services
{
    public class ApiMessages
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Informations { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Warnings { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Errors { get; set; }
    }
}