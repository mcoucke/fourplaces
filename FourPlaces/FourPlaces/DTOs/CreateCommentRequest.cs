using Newtonsoft.Json;

namespace FourPlaces.Dtos
{
	public class CreateCommentRequest
	{
		[JsonProperty("text")]
		public string Text { get; set; }
	}
}