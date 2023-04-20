namespace EDMS.Shared.Constants;

public static class CacheConstants
{
    private static readonly string _seperator = "_";
    public static readonly string Permissions = $"UserPermissions{_seperator}";
    public static readonly string Menus = $"UserMenus{_seperator}";
    public static readonly string AppToken = $"UserAppToken{_seperator}";
    public static readonly string UserToken = $"UserToken{_seperator}";
}
