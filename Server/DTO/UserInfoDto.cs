using EDMS.DSM.Server.Security;

namespace EDMS.DSM.Server.DTO
{
	public class UserInfoDto
	{
		//public long UserId { get; set; }
		public string AspnetUserId { get; set; } = string.Empty;
		public string UserName { get; set; } = string.Empty;
		public string ProgramId { get; set; } = string.Empty;
		public string UserToken { get; set; } = string.Empty;
		public RefreshToken RefreshToken { get; set; } = new();
        public string LatestVersion { get; set; } = string.Empty;
        public long ExpiryTime { get; set; } = 0;
        public int TimeOutMinutes { get; set; } = 0;
    }
	public class Locations
	{
		public string LocationCode { get; set; } = string.Empty;
		public string LocationName { get; set; } = string.Empty;
	}
}
