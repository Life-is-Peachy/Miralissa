using Microsoft.AspNetCore.Components.Authorization;
using Miralissa.Shared;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Miralissa.Client.Services
{
	public class IdentityAuthenticationStateProvider : AuthenticationStateProvider
    {
        private UserInfo _userInfoCache;
		private IAuthenticationService AuthenticationService { get; }

		public IdentityAuthenticationStateProvider(IAuthenticationService service) => AuthenticationService = service;


        public async Task CreateUserAsync(CreateUserModel user)
        {
			await AuthenticationService.CreateUserAsync(user);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task SignInUserAsync(SignInModel user)
        {
            await AuthenticationService.SignInUserAsync(user);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = await GetUserInfoAsync();
                if (userInfo.IsAuthenticated)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, userInfo.UserName) }.Concat(userInfo.ExposedClaims.Select(c => new Claim(c.Key, c.Value)));
                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private async Task<UserInfo> GetUserInfoAsync()
        {
            if (_userInfoCache != null && _userInfoCache.IsAuthenticated)
                return _userInfoCache;

            UserInfo result = await AuthenticationService.GetUserInfoAsync();

            return _userInfoCache = result;
        }
    }
}
