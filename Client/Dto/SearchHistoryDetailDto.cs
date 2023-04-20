namespace EDMS.DSM.Client.Dto;

public class SearchHistoryDetailDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<CommentDetails> Comments { get; set; } = new();
    public List<LclTarifRates> LclTarifRates { get; set; } = new();
    public List<SpecialRates> SpecialRates { get; set; } = new();
    public List<FclTarifRates> FclTarifRates { get; set; } = new();
}

public class LclTarifRates
{
    public string routing { get; set; } = string.Empty;
    public string preference { get; set; } = string.Empty;
    public string sell_wm { get; set; } = string.Empty;
    public int vgm { get; set; } = 0;
    public int thc_per_wm { get; set; } = 0;
    public int thc_per_ton { get; set; } = 0;
    public int thc_min { get; set; } = 0;
}

public class FclTarifRates
{
    public string total_freight_twenty { get; set; } = string.Empty;
    public string total_freight_fourty { get; set; } = string.Empty;
    public string total_freight_fourty_hc { get; set; } = string.Empty;
    public int ihc_twenty { get; set; } = 0;
    public int ens_twenty { get; set; } = 0;
    public int ams_twenty { get; set; } = 0;
    public int vgm_twenty { get; set; } = 0;
    public int doc_twenty { get; set; } = 0;
    public int ebs_twenty { get; set; } = 0;
    public int thc_twenty { get; set; } = 0;
    public int ihc_fourty { get; set; } = 0;
    public int ens_fourty { get; set; } = 0;
    public int ams_fourty { get; set; } = 0;
    public int vgm_fourty { get; set; } = 0;
    public int doc_fourty { get; set; } = 0;
    public int ebs_fourty { get; set; } = 0;
    public int thc_fourty { get; set; } = 0;
    public int ihc_fourty_hc { get; set; } = 0;
    public int ens_fourty_hc { get; set; } = 0;
    public int ams_fourty_hc { get; set; } = 0;
    public int vgm_fourty_hc { get; set; } = 0;
    public int doc_fourty_hc { get; set; } = 0;
    public int ebs_fourty_hc { get; set; } = 0;
    public int thc_fourty_hc { get; set; } = 0;
    public string remark { get; set; } = string.Empty;
    public string preference { get; set; } = string.Empty;
}

public class SpecialRates
{
    public string por { get; set; } = string.Empty;
    public string fdc { get; set; } = string.Empty;
    public string chargecode { get; set; } = string.Empty;
    public string chargename { get; set; } = string.Empty;
    public string currency { get; set; } = string.Empty;
    public string unitcode { get; set; } = string.Empty;
    public string amount { get; set; } = string.Empty;
    public string remark { get; set; } = string.Empty;
}
