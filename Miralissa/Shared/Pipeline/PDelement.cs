using System;

namespace Miralissa.Shared
{
	public record PDelement
	{
		public string Source { get; set; }
		public string Surname { get; set; }
		public string Name { get; set; }
		public string Patronymic { get; set; }
		public string FIO { get { return Surname + " " + Name + " " + Patronymic; } }
		public DateTime? DateBirth { get; set; }
		public string PlaceBirth { get; set; }
		public string INN { get; set; }
		public string SNILS { get; set; }
		public string Address { get; set; }
		public string PassSeria { get; set; }
		public string PassNumber { get; set; }
		public string PassIssueAddr { get; set; }
		public DateTime? PassIssueDate { get; set; }
		public int Rank { get; set; }
	}
}
