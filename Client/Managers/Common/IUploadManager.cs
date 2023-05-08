namespace EDMS.DSM.Client.Managers.Common;

public interface IUploadManager : IManager
{
    Task<IApiResult> UploadFileAsync(string uploadUri, MemoryStream ms, string fileName);
    Task<IListApiResult<List<CommonUploadDto>>> SearchAsync(CommonUploadFilter commonUploadFilter, int orgId);
    Task<IListApiResult<List<CommonUploadDto>>> GetUploadsAsync(int orgId);
    Task<IApiResult> DeleteAsync(int id);
    Task<IApiResult> TransferDataAsync(int id);
    Task<Stream> DownloadErrorFileAsync(string CSVFileName);
    Task<Stream> DownloadSourceFileAsync(string fileName);
    Task DownloadExcelFileAsync<TIn>(TIn communication);
}
