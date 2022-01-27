using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Miralissa.Server.Classes
{
	public static class IdentityExtensions
	{
		public static bool InspectResult(this IdentityResult @this, out string @errors)
		{
			@errors = string.Empty;

			if (@this.Succeeded) return true;

			foreach (string error in @this.Errors.Select(err => err.Description))
				@errors += $"[{error}] ";

			return false;
		}

		public static bool InspectResult(this SignInResult @this, out string @errors)
		{
			@errors = string.Empty;

			if (@this.Succeeded) return true;

			if (@this.IsLockedOut) @errors += "[Аккаунт заблокирован] ";
			if (@this.IsNotAllowed) @errors += "[Авторизация запрещена] ";
			if (@this.RequiresTwoFactor) @errors += "[Необходима двуфакторная авторизация] ";

			return false;
		}
	}
}