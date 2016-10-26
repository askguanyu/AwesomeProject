using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace AwesomeAPI.Services
{
    public class ResponseBucket
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ApiMessages Messages { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ModelStateDictionary ModelState { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Exception Exception { get; set; }
    }
}