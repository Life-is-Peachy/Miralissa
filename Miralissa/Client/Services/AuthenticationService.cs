using Miralissa.Shared;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Miralissa.Client.Services
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly HttpClient _httpClient;

		public AuthenticationService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task CreateUserAsync(CreateUserModel user)
		{
			HttpResponseMessage result = await _httpClient.PostAsJsonAsync<CreateUserModel>("api/authentication/CreateUserAsync", user);
			if (result.StatusCode == HttpStatusCode.BadRequest)
				throw new HttpRequestException(await result.Content.ReadAsStringAsync());

			result.EnsureSuccessStatusCode();
		}

		public async Task SignInUserAsync(SignInModel user)
		{
			HttpResponseMessage result = await _httpClient.PostAsJsonAsync<SignInModel>("api/authentication/SignInUser", user);
			if (result.StatusCode == HttpStatusCode.BadRequest)
				throw new HttpRequestException(await result.Content.ReadAsStringAsync());

			result.EnsureSuccessStatusCode();
		}

		public async Task<UserInfo> GetUserInfoAsync()
			=> await _httpClient.GetFromJsonAsync<UserInfo>("api/authentication/GetUserInfo");
	}
}
