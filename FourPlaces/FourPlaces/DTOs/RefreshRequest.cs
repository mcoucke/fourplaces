using Newtonsoft.Json;

namespace FourPlaces.Dtos
{
    public class RefreshRequest
    {
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}