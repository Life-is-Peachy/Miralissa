namespace Miralissa.Shared
{
	public struct PDmodel
	{
		public string FIO { get; set; }
		public string Address { get; set; }

		public PDmodel(string name, string address)
		{
			FIO = name;
			Address = address;
		}
	}
}
