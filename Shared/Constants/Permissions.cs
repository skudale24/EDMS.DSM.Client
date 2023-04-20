namespace EDMS.Shared.Constants;

public static class Permissions
{
    public static class Agents
    {
        public static readonly string Upload = "agents:upload";
        public static readonly string List = "agents:list";
        public static readonly string Edit = "agents:edit";
        public static readonly string Delete = "agents:delete";
    }

    public static class Quote
    {
        public static readonly string ManualQuoteDap = "roadbridge:dapswitchtomanual";
        public static readonly string ManualQuoteExWorks = "roadbridge:exworksswitchtomanual";
    }

    public static class ImportPricing
    {
        public static readonly string MmsrgLcl = "importpricing:msrglcl";
        public static readonly string BuyFcl = "importpricing:buyfcl";
    }

    public static class ExportPricing
    {
        public static readonly string MmsrgLcl = "exportpricing:msrglcl";
        public static readonly string BuyFcl = "exportpricing:buyfcl";
        public static readonly string SellLcl = "exportpricing:selllcl";
        public static readonly string SellFcl = "exportpricing:sellfcl";
    }
}
