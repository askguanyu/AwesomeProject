using AwesomeLib.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AwesomeLib.Services
{
    public class Converter : IConverter
    {
        public string ConvertToJson(object data)
        {
            return ConvertToJson(data, true, false);
        }

        public string ConvertToJson(object data, bool camelCase, bool indented)
        {
            var settings = new JsonSerializerSettings()
            {
                Formatting = indented ?
                    Formatting.Indented :
                    Formatting.None,
                ContractResolver = camelCase ?
                    new CamelCasePropertyNamesContractResolver() :
                    new DefaultContractResolver()
            };

            return JsonConvert.SerializeObject(data, settings);
        }
    }
}