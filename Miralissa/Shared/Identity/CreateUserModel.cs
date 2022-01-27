namespace Miralissa.Shared
{
	public record CreateUserModel
	{
		public UserCredentials Credentials {  get; set; }
		public ClaimsCredentials Claims { get; set; }
	}
}
