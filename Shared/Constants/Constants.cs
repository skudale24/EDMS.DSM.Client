namespace EDMS.Shared.Constants;

public static class AppConstants
{
    public const string AppTokenHeaderKey = "AppToken";
    public const string UserTokenHeaderKey = "UserToken";
    public const string UserRefreshTokenHeaderKey = "RefToken";
}

public static class StorageConstants
{
    public static string UserToken = "_z";
    public static string RefreshToken = "_r";
    public static string Email = "email";
    public static string FullName = "fullName";
    public static string Permissions = "permissions";
    public static string Version = "version";
    public static string Expiry = "expiry";
    public static string PageSize = "pagesize";
    public static string UserId = "userId";
    public static string ProgramId = "programId";
    public static string AspNetUserId = "aspuserId";
}

public static class SettingKeyConstants
{
    private static readonly string GemBoxKey = "GemBoxKey";

    private static readonly string UploadFilePath = "UploadFilePath";

    private static readonly string PrivateKey = "PrivateKey";

    public static string GetSettingKey(SettingKeys settingKey)
    {
        return settingKey switch
        {
            SettingKeys.GemBoxKey => GemBoxKey,
            SettingKeys.UploadFilePath => UploadFilePath,
            SettingKeys.PrivateKey => PrivateKey,
            _ => string.Empty
        };
    }
}
