namespace EDMS.DSM.Client.Managers.Common;

public interface ILookupManager : IManager
{
    Task<bool> ValidatePincodeAsync(string pincode);
}
