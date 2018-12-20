using MPK.Connect.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MPK.Connect.Model.Business
{
    public class RouteDto
    {
        public string Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RouteTypes Type { get; set; }
    }
}