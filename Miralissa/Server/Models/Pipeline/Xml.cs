namespace Miralissa.Server
{
    public record Xml
    {
        public int IdPipeline { get; set; }
        public byte[] XmlData { get; set; }
        public byte[] HtmlData { get; set; }
        public string XslPath { get; set; }
        public int? XmlSize { get; set; }
        public int? HtmlSize { get; set; }
    }
}
