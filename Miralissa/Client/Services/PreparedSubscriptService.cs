using Miralissa.Shared;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Miralissa.Client.Services
{
	public class PreparedSubscriptService : IPreparedService
    {
        private readonly HttpClient _httpClient;

		public PreparedSubscriptService(HttpClient client)
		{
			_httpClient = client;
		}

		public async Task MarkAsync(int[] iDs)
		{
			HttpResponseMessage result = await _httpClient.PostAsJsonAsync<int[]>("api/prepared/MarkAsOrderable", iDs);
			if (!result.IsSuccessStatusCode)
				throw new HttpRequestException(await result.Content.ReadAsStringAsync());
		}

		public async Task DeleteAsync(PreparedSubscript subscript)
		{
			HttpResponseMessage result = await _httpClient.PostAsJsonAsync<PreparedSubscript>("api/prepared/DeletePrepared", subscript);
			if (!result.IsSuccessStatusCode)
				throw new HttpRequestException(await result.Content.ReadAsStringAsync());
		}

		public async Task CreateAsync(PreparedSubscript subscript)
		{
			HttpResponseMessage result = await _httpClient.PostAsJsonAsync<PreparedSubscript>("api/prepared/CreateCadastralAsync", subscript);
			if (!result.IsSuccessStatusCode)
				throw new HttpRequestException(await result.Content.ReadAsStringAsync());
		}

		public async Task<ICollection<PreparedSubscript>> GetAsync()
			=> await _httpClient.GetFromJsonAsync<ICollection<PreparedSubscript>>("api/prepared/Get");
	}
}
