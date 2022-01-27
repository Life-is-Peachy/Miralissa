using System;

namespace Miralissa.Shared
{
	public record ClaimsCredentials
	{
		public string Forename { get; set; }
		public string Surname { get; set; }
		public string Patronymic { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public Gender Gender { get; set; }
		public string MobilePhone { get; set; }
	}
}
