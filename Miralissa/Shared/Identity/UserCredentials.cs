namespace Miralissa.Shared
{
	public record UserCredentials
	{
		public string Login { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string WorkPhone { get; set; }
		public bool Remember { get; set; }
	}
}
