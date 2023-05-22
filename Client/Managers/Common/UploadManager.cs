﻿using EDMS.DSM.Client.Pages.Customer;
using EDMS.DSM.Shared.Models;
using System.Net;

namespace EDMS.DSM.Client.Managers.Common;

public class UploadManager : IUploadManager
{
    private readonly HttpRequest _httpRequest;

    public UploadManager(HttpRequest httpRequest)
    {
        _httpRequest = httpRequest;
    }

    public async Task<IApiResult> UploadFileAsync(string uploadUri, MemoryStream ms, string fileName)
    {
        var urlWithParams = uploadUri;

        var res = await _httpRequest.PostMultiPartRequestAsync(urlWithParams, ms, fileName).ConfigureAwait(false);

        return res;
    }

    public Task<IListApiResult<List<CommonUploadDto>>> SearchAsync(CommonUploadFilter commonUploadFilter, int orgId)
    {
        var queryString = HttpExtensions.GenerateQueryString(commonUploadFilter);
        var urlWithParams = $"{UploadEndPoints.GetUploads}/{orgId}/?{queryString}";

        return _httpRequest.GetRequestAsync<List<CommonUploadDto>>(urlWithParams);
    }

    public Task<IListApiResult<List<CommonUploadDto>>> GetUploadsAsync(int orgId)
    {
        var urlWithParams = $"{UploadEndPoints.GetUploads}/{orgId}/";
        return _httpRequest.GetRequestAsync<List<CommonUploadDto>>(urlWithParams);
    }

    public Task<IApiResult> DeleteAsync(int id)
    {
        var url = $"{UploadEndPoints.Delete}/{id}";
        return _httpRequest.DeleteRequestAsync(url);
    }

    public async Task<IApiResult> TransferDataAsync(int id)
    {
        var url = string.Format(UploadEndPoints.TransferData, id);
        var response = await _httpRequest.PostRequestAsync<string, ApiResult>(url, string.Empty).ConfigureAwait(false);

        return response;
    }

    public async Task<Stream> DownloadErrorFileAsync(string CSVFileName)
    {
        var urlWithParams = $"{UploadEndPoints.DownloadErrorFile}/{CSVFileName}/";
        var stream = await _httpRequest.GetStreamAsync(urlWithParams).ConfigureAwait(false);

        return stream;
    }

    public async Task<Stream> DownloadSourceFileAsync(string fileName)
    {
        var encodedFileName = WebUtility.UrlEncode(fileName);
        var urlWithParams = $"{UploadEndPoints.DownloadSourceFile}/{encodedFileName}/";
        var stream = await _httpRequest.GetStreamAsync(urlWithParams).ConfigureAwait(false);
        return stream;
    }

    public async Task<Stream> DownloadExcelFileAsync<TIn>(TIn communication)
    {
        var urlWithParams = $"{UploadEndPoints.DownloadExcelFile}";
        var stream = await _httpRequest.GetStreamDataAsync<TIn>(urlWithParams, communication).ConfigureAwait(false);
        return stream;
    }

    public async Task<Stream> ExportGridAsync<TIn>(TIn communications, List<GridColumnDTO> gridColumns)
    {
        var urlWithParams = $"{UploadEndPoints.ExportGrid}";

        var data = new
        {
            Communications = communications,
            GridColumns = gridColumns
        };

        var stream = await _httpRequest.GetStreamDataAsync(urlWithParams, data).ConfigureAwait(false);
        return stream;
    }
}
