using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Miralissa.Server.Classes;
using Miralissa.Server.Models;
using Miralissa.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Miralissa.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;

		public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> CreateUserAsync(CreateUserModel model)
		{
			User user = new()
			{
				UserName = model.Credentials.Login,
				Email = model.Credentials.Email,
				EmailConfirmed = true,
				PhoneNumber = model.Credentials.WorkPhone
			};

			var createResult = await _userManager.CreateAsync(user, model.Credentials.Password);
			var createSuccessed = createResult.InspectResult(out string errors);

			if (!createSuccessed)
				return BadRequest(errors);

			var signInResult = await SignInUserAsync(new SignInModel
			{
				Login = model.Credentials.Login,
				Password = model.Credentials.Password
			});

			if (signInResult is not OkResult)
				return signInResult as BadRequestObjectResult;

			IActionResult setClaimsResult = await SetClaimsOnCreateAsync();

			if (setClaimsResult is not OkResult)
				return setClaimsResult as BadRequestObjectResult;

			return Ok();

			async Task<IActionResult> SetClaimsOnCreateAsync()
			{
				List<Claim> claimList = new()
				{
					new Claim(ClaimTypes.GivenName, model.Claims.Forename, ClaimValueTypes.String, "UserSelf"),
					new Claim(ClaimTypes.Surname, model.Claims.Surname, ClaimValueTypes.String, "UserSelf"),
					new Claim("Patronymic", model.Claims.Patronymic, ClaimValueTypes.String, "UserSelf"),
					new Claim(ClaimTypes.DateOfBirth, model.Claims.DateOfBirth.Value.ToString("dd.MM.yyyy"), ClaimValueTypes.Date, "UserSelf"),
					new Claim(ClaimTypes.Gender, model.Claims.Gender.ToString(), ClaimValueTypes.String, "UserSelf"),
					new Claim(ClaimTypes.MobilePhone, model.Claims.MobilePhone, ClaimValueTypes.String, "UserSelf")
				};

				var setClaimsResult = await _userManager.AddClaimsAsync(user, claimList);
				return setClaimsResult.InspectResult(out string errors)
					? Ok()
					: BadRequest(errors);
			}
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> SignInUserAsync(SignInModel model)
		{
			User user = await _userManager.FindByNameAsync(model.Login);
			if (user is null)
				return BadRequest("Пользователь не существует");

			var singInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

			await _signInManager.SignInAsync(user, true);

			return singInResult.InspectResult(out string errors)
				? Ok()
				: BadRequest(errors);
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> SetClaimsAsync(ClaimsCredentials claims)
		{
			User user = await _userManager.GetUserAsync(User);

			List<Claim> claimList = new()
			{
				new Claim(ClaimTypes.GivenName, claims.Forename, ClaimValueTypes.String, "UserSelf"),
				new Claim(ClaimTypes.Surname, claims.Surname, ClaimValueTypes.String, "UserSelf"),
				new Claim("Patronymic", claims.Patronymic, ClaimValueTypes.String, "UserSelf"),
				new Claim(ClaimTypes.DateOfBirth, claims.DateOfBirth.Value.ToString("dd.MM.yyyy"), ClaimValueTypes.Date, "UserSelf"),
				new Claim(ClaimTypes.Gender, claims.Gender.ToString(), ClaimValueTypes.String, "UserSelf"),
				new Claim(ClaimTypes.MobilePhone, claims.MobilePhone, ClaimValueTypes.String, "UserSelf")
			};

			var setClaimsResult = await _userManager.AddClaimsAsync(user, claimList);

			return setClaimsResult.InspectResult(out string errors)
				? Ok()
				: BadRequest(errors);
		}

		[HttpGet("[action]")]
		public UserInfo GetUserInfo()
			=> new UserInfo
			{
				IsAuthenticated = User.Identity.IsAuthenticated,
				UserName = User.Identity.Name,
				ExposedClaims = User.Claims.ToDictionary(c => c.Type, c => c.Value)
			};
	}
}
