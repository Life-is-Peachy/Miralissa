using System;

namespace Miralissa.Server
{
    public record Egrp
    {
        public int Id { get; set; }
        public int IdPipeline { get; set; }
        public string Fio { get; set; }
        public DateTime? RegDate { get; set; }
        public int? Numerator { get; set; }
        public int? Denominator { get; set; }
    }
}
