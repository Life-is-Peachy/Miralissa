using System;

namespace Miralissa.Server
{
    public record Subscript
    {
        public int ID { get; set; }
        public int? ID_Request { get; set; }
        public string Source { get; set; }
        public DateTime AddressRecivedAt { get; set; }
        public int Priority { get; set; }
        public string Worker { get; set; }
        public string CadastralNumber { get; set; }
        public string Address { get; set; }
        public string Square { get; set; }
        public string NumRequest { get; set; }
        public DateTime? RequestRecivedAt { get; set; }
        public DateTime? LastUploadAttempt { get; set; }
        public string Result { get; set; }
        public bool HasXml { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string Home { get; set; }
        public string Corp { get; set; }
        public string Flat { get; set; }
        public string R_FullAddress { get; set; }
        public string R_ObjType { get; set; }
        public string R_Square { get; set; }
        public string R_SteadCategory { get; set; }
        public string R_SteadKind { get; set; }
        public string R_FuncName { get; set; }
        public string R_Status { get; set; }
        public decimal? R_CadastralCost { get; set; }
        public DateTime? R_CadastralCostDate { get; set; }
        public string R_NumStoreys { get; set; }
        public DateTime? R_UpdateInfoDate { get; set; }
        public string R_LiterBTI { get; set; }
        public bool IsChecked { get; set; }
        public bool IsResult { get; set; }
    }
}
