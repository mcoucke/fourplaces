using Newtonsoft.Json;

namespace FourPlaces.Dtos
{
	public class ImageItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }
	}
}