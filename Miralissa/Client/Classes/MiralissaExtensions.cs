using Miralissa.Client.Classes;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Miralissa.Client
{
	public static class MiralissaExtensions
	{
		public static async Task<bool> TryActionAsync<T>(this T @this, Func<T, Task> @onTry)
		{
			try
			{
				await @onTry(@this);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static async Task<bool> TryActionNotifiableAsync<T>(this T @this, Func<T, Task> @onTry, ISnackbar @snackbar = null, string @successMsg = "Успех")
		{
			try
			{
				await @onTry(@this);
				@snackbar.NotifySuccess(@successMsg);
				return true;
			}
			catch (Exception ex)
			{
				@snackbar?.NotifyError(ex.Message);
				return false;
			}
		}

		public static async Task TryActionThrowableAsync<T>(this T @this, Func<T, Task> @try)
		{
			try { await @try(@this); }
			catch { throw; }
		}

		public static void DoNotAwait(this Task @this)
		{ }
	}
}