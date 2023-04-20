namespace EDMS.DSM.Client.Routes;

public static class EndPoints
{
    //public static string Version = "0.9.0014";
    //public static string ApiBaseUrl => "https://localhost:7112";
    //public static string LoginPage => "https://localhost:7119";
    //public static string PhotosBaseUrl => "http://localhost:5119";

    //iworktech server
    public static string Version = "0.9.0014";
    public static string ApiBaseUrl => "https://tivaapi.iworktech.net";
    public static string LoginPage => "https://logsoft.iworktech.net";
    public static string PhotosBaseUrl => "https://pwa.iworktech.net";
}

public static class NavMenuEndPoints
{
    public const string Menus = "api/v1/user/menus";
    public const string UserData = "api/v1/user/userdata";
}

public static class PincodeEndPoints
{
    public const string Search = "api/v1/pincode/search";
    public const string IsValidPincode = "api/v1/pincode/isvalid";
}

public static class TransporatationEndPoints
{
    public const string Search = "api/v1/transportation/search";
}

public static class PostLandingCostEndPoints
{
    public const string Search = "api/v1/postlanding/search";
}

public static class ExWorkEndPoints
{
    public const string Search = "api/v1/exworks/search";
}

public static class UserEndPoints
{
    public const string Organizations = "api/v1/user/organizations";
    public const string IsUserTokenValid = "api/v1/user/token/valid";
    public const string RefreshUserToken = "api/v1/usermanage/refresh";
}

public static class TermsEndPoints
{
    public const string Terms = "api/v1/terms";
}

public static class QuoteSearch
{
    public const string Search = "api/v1/quote/searchreport";
    public const string ExportReport = "api/v1/quote/exportquotesearch";
    public const string MarkAsExecuted = "api/v1/quote/executehistory";
    public const string MarkAsClosed = "api/v1/quote/closehistory";
    public const string AddComments = "api/v1/quote/addcomments";
    public const string DownloadPdfFile = "api/v1/quote/downloadpdffile";
    public const string UpdateEfreightNo = "api/v1/quote/efreight";
}

public static class BuyFclImportEndPoints
{
    public const string Search = "api/v1/pricing/fclimport/search";
}

public static class BuyLclImportEndPoints
{
    public const string Search = "api/v1/pricing/lclimport/search";
}

public static class BuyLclExportEndPoints
{
    public const string Search = "api/v1/pricing/lclexport/search";
}

public static class BuyFclExportUsaEndPoints
{
    public const string Search = "api/v1/pricing/fclexportusa/search";
}

public static class BuyFclExportNonUsaEndPoints
{
    public const string Search = "api/v1/pricing/fclexportnonusa/search";
}

public static class SellLclExportEndPoints
{
    public const string Search = "api/v1/pricing/selllclexport/search";
}

public static class SellFclExportUsaEndPoints
{
    public const string Search = "api/v1/pricing/sellfclexportusa/search";
}

public static class SellFclExportNonUsaEndPoints
{
    public const string Search = "api/v1/pricing/sellfclexportnonusa/search";
}

public static class MarkupHistoryEndpoints
{
    public const string Search = "api/v1/pricing/markuphistory/search";
}

public static class SearchHistoryEndPoints
{
    public const string GetFromId = "api/v1/pricing/searchhistoryreport";
    public const string Search = "api/v1/pricing/searchhistoryreport/search";
    public const string MarkAsClosed = "api/v1/pricing/closehistory";
    public const string MarkAsExecuted = "api/v1/pricing/executehistory";
    public const string AddComments = "api/v1/pricing/addcomments";
}

public static class UploadEndPoints
{
    public const string UploadPricing = "api/v1/pricing/upload";
    public const string Upload = "api/v1/upload";
    public const string GetUploads = "api/v1/pricing/uploads";
    public const string Delete = "api/v1/pricing/upload";
    public const string TransferData = "api/v1/pricing/upload/{0}/transferdata";
    public const string DownloadErrorFile = "api/v1/pricing/downloaderrorfile";
    public const string DownloadSourceFile = "api/v1/pricing/downloadsourcefile";
}

public static class QuoteEndPoints
{
    public const string GetQuote = "api/v1/quote";
    public const string SubmitQuote = "api/v1/quote/submit";
    public const string BookMailQuote = "api/v1/quote/quotebookmail";
    public const string PrintQuote = "api/v1/quote/quoteprint";
    public const string IsValid = "api/v1/quote/isvalid?pincode={0}";
}

public static class PhotosEndPoints
{
    public const string Search = "api/v1/photos/search";
    public const string GetFromId = "api/v1/photos/{0}";
    public const string UpdateContainerNo = "api/v1/photos/containerNo/{0}?containerNo={1}";
    public const string UpdateShippingBillNo = "api/v1/photos/shippingBillNo/{0}?shippingBillNo={1}";
    public const string DeleteCateory = "api/v1/photos/deletecategorydetail/{0}";
    public const string DeletePhoto = "api/v1/photos/deletecategoryimage/{0}";
}

public static class PricingEndPoints
{
    public const string OriginSearch = "api/v1/tivapricing/origin/search";
    public const string DestinationSearch = "api/v1/tivapricing/destination/search";
    public const string ImportMSRGLCL = "api/v1/tivapricing/importmsgrlcl/search";
    public const string ImportBuyFCL = "api/v1/tivapricing/importbuyfcl/search";
    public const string ExportMSRGLCL = "api/v1/tivapricing/exportmsrglcl/search";
    public const string ExportSellLCL = "api/v1/tivapricing/exportselllcl/search";
    public const string ExportBuyFCL = "api/v1/tivapricing/exportbuyfcl/search";

    public const string ExportSellFCL = "api/v1/tivapricing/exportsellfcl/search";

    //BookMailQuote
    public const string BookMailQuote = "api/v1/pricing/pricebookmail";

    //Markupemail
    public const string PricingLCLMarkupEmail = "api/v1/tivapricing/pricinglclmarkupemail";
    public const string PricingFCLMarkupEmail = "api/v1/tivapricing/pricingfclmarkupemail";

    public const string ExportPriceReport = "api/v1/pricing/exportpricesearch";
}

public static class AgentsEndPoints
{
    //External
    public const string CountrySearch = "api/v1/tivaagents/country/search";

    public const string CitySearch = "api/v1/tivaagents/city/search";


    //Internal
    public const string Search = "api/v1/tivaagents/search";
    public const string Get = "api/v1/tivaagents/get";
    public const string Update = "api/v1/tivaagents/update";
    public const string DeleteAgentById = "api/v1/tivaagents/delete/{0}";
}

public static class SailingScheduleEndpoints
{
    public const string OriginSearch =
        "https://www.wwalliance.com/webservice/rest/scheduleorigin/origin.json?SenderID=Teamglobal_webservices&Password=9VrPzFsBBv@fec";

    public const string DestinationSearch = "https://www.wwalliance.com/wwaonline/sch/destinationlist.php?";

    public const string Search =
        "https://www.wwalliance.com/webservice/rest/schedule/search.json?SenderID=Teamglobal_webservices&Password=9VrPzFsBBv@fec&Version=3.0.1";
}

public static class LabelEndPoints
{
    public const string AirLabelAdd = "api/v1/label/addairlabel";
    public const string SeaLabelAdd = "api/v1/label/addsealabel";
    public const string GetPackages = "api/v1/label/getpackages";
    public const string LabelShippingBillUrl = "api/v1/label/getshippingdata";
}

public static class FeedbackPoints
{
    public const string FeedbackEmail = "api/v1/tivafeedback/feedbackmail";
}

public static class TrackShipmentEndpoints
{
    public const string HBLNumber =
        "https://ecommerce.teamglobal.in/ws_trackntrace/shipmentstatus.asmx/HBLDetails?hblno=";

    public const string ContainerNumber =
        "https://ecommerce.teamglobal.in/ws_trackntrace/shipmentstatus.asmx/ContainerDetails?containerno=";

    public const string ShippingBillNumber =
        "https://ecommerce.teamglobal.in/ws_trackntrace/api_service.asmx/booking?shippingBill=";

    public const string HAWBNumber =
        "https://ecommerce.teamglobal.in/ws_trackntrace/shipmentstatus.asmx/HAWBDetails?hawbno=";

    public const string MAWBNumber =
        "https://ecommerce.teamglobal.in/ws_trackntrace/shipmentstatus.asmx/MAWBDetails?mawbno=";
}

public static class ContainerPhotosEndpoints
{
    public const string GetAllImages = "api/v1/customphotos/getallimages?CategoryName={0}&ContainerNo={1}";
    public const string DeletePhoto = "api/v1/photos/deletecategoryimage/{0}";
}

public static class ShippingPhotosEndpoints
{
    public const string GetAllImages = "api/v1/customphotos/getallimages?CategoryName={0}&ShippingBillNo={1}";
    //public const string DeletePhoto = "api/photos/deletecategoryimage/{0}"; 
}

public static class BannerEndPoints
{
    public const string Search = "api/v1/banner/search";
    public const string Get = "api/v1/banner/getbannersdetails";
    public const string DeleteBannerById = "api/v1/banner/delete/{0}";
    public const string Update = "api/v1/banner/update";
    public const string UpdateDisplayOrderNo = "api/v1/banner/displayorderNo/{0}?displayorderNo={1}";
}

public static class ReportsEndpoints
{
    public const string ExportReport = "api/v1/label/ExportSeaLable";
    public const string ExportAirLableReport = "api/v1/label/ExportAirLable";
}
