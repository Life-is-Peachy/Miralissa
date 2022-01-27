using MudBlazor;
using System.Collections.Generic;
using System.Linq;

namespace Miralissa.Client.Classes
{
	public static class SnackbarExtensions
	{
		public static Snackbar AddRange(this ISnackbar snackbar, IEnumerable<string> messages)
		{
			if (messages is null || messages == Enumerable.Empty<string>())
				return snackbar.Add(string.Empty);

			string summary = string.Empty;
			foreach (string message in messages)
				summary += $" [{message}]";

			return snackbar.Add(summary);
		}

		public static void NotifyInfo(this ISnackbar @snackbar, string msg)
			=> snackbar.Add(msg, Severity.Info);

		public static void NotifyWarning(this ISnackbar @snackbar, string msg)
			=> snackbar.Add(msg, Severity.Warning);

		public static void NotifyError(this ISnackbar @snackbar, string msg)
			=> snackbar.Add(msg, Severity.Error);

		public static void NotifySuccess(this ISnackbar @snackbar, string msg)
			=> snackbar.Add(msg, Severity.Success);
	}
}
