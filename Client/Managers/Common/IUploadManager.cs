namespace EDMS.DSM.Client.Managers.Common;

public interface IUploadManager : IManager
{
    Task<IApiResult> UploadFileAsync(string uploadUri, MemoryStream ms, string fileName);
    Task<IApiResult> DeleteAsync(int id);
    Task<IApiResult> TransferDataAsync(int id);
    Task<Stream> DownloadSourceFileAsync(string fileName);
    Task<Stream> DownloadExcelFileAsync<TIn>(TIn communication);
    Task<Stream> ExportGridAsync<TIn>(TIn communications, List<GridColumnDTO> gridColumns);
}
