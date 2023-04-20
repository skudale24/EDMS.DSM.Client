namespace EDMS.Shared.Enums;

// TODO: Document (or rename if necessary) `PhotoFilterCategory` enum properly, once the required specifications received.

/// <summary>
/// </summary>
public enum PhotoFilterCategory
{
    Carting,
    Empty,
    Sealed,
    Stuffing,
    DeStuffing,
    HazContainerLabel
}

public enum PricingUploadFileStatus
{
    Processing,
    Ready,
    Completed,
    Failed
}

public enum PricingUploadFileType
{
    BuyFCLImports,
    BuyLCLImports,
    BuyLCLExports,
    BuyFCLExportsusa,
    BuyFCLExportsnonusa,
    SellLCLExports,
    SellFCLExportsusa,
    SellFCLExportsnonusa
}
