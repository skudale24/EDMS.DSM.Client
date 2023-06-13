namespace EDMS.DSM.Client.Routes;

public static class EndPoints
{

#if DEBUG
    //Visual Studio Dev mode
    public static string Version = "0.1.0021";
    public static string ApiBaseUrl => "";
    public static string APBaseUrl => "http://localhost:53398";
    public static string LoginPage => "";

#else
    //Dev server
    public static string Version = "0.1.0021";
    public static string ApiBaseUrl => "";
    public static string APBaseUrl => "https://esadmin.energydatametrics.com";
    public static string LoginPage => "";

#endif

}

public static class NavMenuEndPoints
{
    public const string Menus = "api/v1/user/menus";
    public const string UserData = "api/v1/user/userdata";
}

public static class UserEndPoints
{
    public const string Organizations = "api/v1/user/organizations";
    public const string IsUserTokenValid = "api/v1/user/token/valid";
    public const string RefreshUserToken = "api/authentication/refresh";
    public const string RegenerateUserToken = "api/authentication/regenerate";
}

public static class UploadEndPoints
{
    public const string Upload = "api/v1/upload";
    public const string Delete = "api/v1/pricing/upload";
    public const string TransferData = "api/v1/pricing/upload/{0}/transferdata";
    public const string DownloadSourceFile = "api/customer/downloadsourcefile";
    public const string DownloadExcelFile = "api/customer/downloadexcelfile";
    public const string ExportGrid = "api/customer/exportgrid";
}

public static class CustomerEndpoints
{
    public const string GetCommunications = "api/customer/list";
    public const string GenerateLetter = "api/customer/generateletter";
}
