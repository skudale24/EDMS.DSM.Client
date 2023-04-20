namespace EDMS.DSM.Client.Components.ClipBordFeature;

public interface IClipboardService
{
    Task CopyToClipboard(string text);
}
