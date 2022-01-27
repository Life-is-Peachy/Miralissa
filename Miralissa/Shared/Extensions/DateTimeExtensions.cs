using System;

namespace Miralissa.Shared
{
	public static class DateTimeExtensions
	{
		public static string OnDateNull(this DateTime? @this, string @onNull, string @format = "dd.MM.yyyy")
			=> @this is not null
				? @this.Value.ToString(@format)
				: @onNull;
	}
}
