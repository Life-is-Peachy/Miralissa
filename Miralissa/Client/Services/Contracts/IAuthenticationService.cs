using Microsoft.AspNetCore.Components.Authorization;
using Miralissa.Shared;
using System.Threading.Tasks;

namespace Miralissa.Client.Services
{
	public interface IAuthenticationService
	{
		Task CreateUserAsync(CreateUserModel user);
		Task<UserInfo> GetUserInfoAsync();
		Task SignInUserAsync(SignInModel user);
	}
}