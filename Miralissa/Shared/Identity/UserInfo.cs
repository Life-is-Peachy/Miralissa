using System.Collections.Generic;

namespace Miralissa.Shared
{
	public record UserInfo
	{
		public bool IsAuthenticated { get; set; }
		public string UserName { get; set; }
		public string Source { get; set; }
		public Dictionary<string, string> ExposedClaims { get; set; }
	}
}
