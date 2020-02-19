using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System;
using System.Diagnostics;
using FourPlaces.Dtos;

namespace FourPlaces.Services
{
	public class ApiClient
    {
		private readonly HttpClient _client = new HttpClient();

		public async Task<HttpResponseMessage> Execute(HttpMethod method, string url, object data = null, string token = null)
		{
			HttpRequestMessage request = new HttpRequestMessage(method, url);

			if (data != null)
			{
				request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
			}

			if (token != null)
			{
				request.Headers.Add("Authorization", $"Bearer {token}");
			}

			return await _client.SendAsync(request);
		}

		public async Task<T> ReadFromResponse<T>(HttpResponseMessage response)
		{
			string result = await response.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<T>(result);
		}

		public async Task<HttpResponseMessage> UploadImage(HttpMethod method, string url, byte[] imageData, string token)
		{
			HttpRequestMessage request = new HttpRequestMessage(method, url);
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

			MultipartFormDataContent requestContent = new MultipartFormDataContent();

			var imageContent = new ByteArrayContent(imageData);
			imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

			requestContent.Add(imageContent, "file", "file.jpg");

			request.Content = requestContent;

			return await _client.SendAsync(request);
		}

		internal Task<HttpResponseMessage> UploadImage(object patch, string url, byte[] imageAsBytes, string v)
		{
			throw new NotImplementedException();
		}
	}
}
