namespace EDMS.DSM.Client.Utils;

public static class Constants
{
    public const string Import = "Import";
    public const string Export = "Export";
    public static readonly string[] ImportOriginExcludeCountries = { "India" };
    public static readonly string[] ImportDestinationCountries = { "India", "Bangladesh", "Kenya", "Tanzania" };
    public static readonly string[] ExportOriginCountries = { "India", "Bangladesh", "Kenya", "Tanzania" };
    public static readonly string[] ExportDestinationExcludeCountries = { "India", "Bangladesh", "Kenya", "Tanzania" };

    public static readonly string[] ExportOriginCountries_1 = { "IN", "BD" };
}
