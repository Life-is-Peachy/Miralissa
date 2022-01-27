namespace Miralissa.Server
{
    public record RosrKey
    {
        public int Id { get; set; }
        public string LoginKey { get; set; }
        public string Value { get; set; }
        public string InOrdering { get; set; }
        public string InLoading { get; set; }
    }
}
