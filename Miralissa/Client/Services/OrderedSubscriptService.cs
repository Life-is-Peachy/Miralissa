using Miralissa.Shared;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;

namespace Miralissa.Client.Services
{
	public class OrderedSubscriptService : IOrderedService
	{
		private readonly HttpClient _httpClient;

		public OrderedSubscriptService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task MarkAsync(int ID)
		{
			HttpResponseMessage result = await _httpClient.PostAsJsonAsync<int>("api/ordered/MarkAsync", ID);
			if (!result.IsSuccessStatusCode)
				throw new HttpRequestException(await result.Content.ReadAsStringAsync());
		}

		public async Task DeleteAsync(OrderedSubscript subscript)
		{
			HttpResponseMessage result = await _httpClient.PostAsJsonAsync<OrderedSubscript>("api/ordered/DeleteAsync", subscript);
			if (!result.IsSuccessStatusCode)
				throw new HttpRequestException(await result.Content.ReadAsStringAsync());
		}

		public async Task<ICollection<OrderedSubscript>> GetAsync()
			=> await _httpClient.GetFromJsonAsync<ICollection<OrderedSubscript>>("api/ordered/Get");

		public async Task<string> GetHtml(int ID)
			=> await _httpClient.GetStringAsync($"api/ordered/GetHtml/{ID}");
	}
}
