namespace Miralissa.Shared
{
    public record OrderedSubscript
    {
        public int ID { get; set; }
        public int? ID_Request { get; set; }
        public string Source { get; set; }
        public string Address { get; set; }
        public string CadastralNumber { get; set; }
        public string Result { get; set; }
		public bool IsChecked { get; set; }
		public bool IsResult { get; set; }
    }
}
