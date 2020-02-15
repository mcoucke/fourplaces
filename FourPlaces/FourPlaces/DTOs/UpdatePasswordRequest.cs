using Newtonsoft.Json;

namespace FourPlaces.Dtos
{
    public class UpdatePasswordRequest
    {
        [JsonProperty("old_password")]
        public string OldPassword { get; set; }
        
        [JsonProperty("new_password")]
        public string NewPassword { get; set; }
    }
}