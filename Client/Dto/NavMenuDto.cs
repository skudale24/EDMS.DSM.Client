namespace EDMS.DSM.Client.DTO;

public class NavMenuDto
{
    public int MenuId { get; set; }
    public string MenuCode { get; set; } = string.Empty;
    public string MenuName { get; set; } = string.Empty;
    public int ParentMenuId { get; set; }
    public string Icon { get; set; } = string.Empty;
    public string LinkUrl { get; set; } = string.Empty;
    public int Sequence { get; set; }
    public NavMenuDto[] Submenus { get; set; } = Array.Empty<NavMenuDto>();
    public List<CachePermissionsDto> Permissions { get; set; } = new();
}

public class CachePermissionsDto
{
    public int MenuId { get; set; }
    public string PermissionCode { get; set; } = string.Empty;
    public string PermissionName { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
}

public class UserInfoDto
{
    public long UserId { get; set; }
    public string AspnetUserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public long OrgId { get; set; }
    public string UserToken { get; set; } = string.Empty;
    public RefreshToken RefreshToken { get; set; } = new();
    public string LatestVersion { get; set; } = string.Empty;
    public long ExpiryTime { get; set; } = 0;
    public bool IsInternal { get; set; }
    public List<object> yardLocations { get; set; }
}

//public class RefreshToken
//{
//    public string Token { get; set; }
//    public DateTime Created { get; set; }
//    public DateTime Expires { get; set; }

//}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class RefreshToken
{
    public string token { get; set; }
    public DateTime created { get; set; }
    public DateTime expires { get; set; }
}

public class TokenResult
{
    public int userId { get; set; }
    public string aspnetUserId { get; set; }
    public string userName { get; set; }
    public string emailAddress { get; set; }
    public int orgId { get; set; }
    public string userToken { get; set; }
    public RefreshToken refreshToken { get; set; }
    public string latestVersion { get; set; }
    public long expiryTime { get; set; }
    public bool isInternal { get; set; }
    public List<object> yardLocations { get; set; }
}

public class Root
{
    public TokenResult result { get; set; }
    public bool status { get; set; }
    public string message { get; set; }
}
