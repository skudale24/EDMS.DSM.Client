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
    public static string AspNetUserId = "aspuserId";
}

public static class CSVSampleConstants
{
    private static readonly string agents =
        "Segment,Dest Country,Dest CFS,Customer Name,Address,Phone No,Fax No,Email,Category Code,Hub Agent,Routing,Code";

    private static readonly string pincodes = "pincode,taluka,district,state";

    private static readonly string exworks =
        "Origin,Destination,Custom Clearance,ACD / ACI / ENS Per  HBL,IHC / THC Per W/M,Documentation Per HBL,1 ton - 2 cbm,3 tons - 6 cbm,5 tons - 10 cbm,7 tons - 14 cbm";

    private static readonly string postlanding =
        "Destination,Custom Clearance Charges (USD),CFS Charges Per CBM (USD),PLC Per W/M (USD),Transport,GST";

    private static readonly string transportation =
        "Country,POD,Origin State,Origin Region,From zip code,To zip code,Destination State,Destination Region,Applicable Slab Zone,Per Kg (INR),1 ton - 2 cbm,3 tons - 6 cbm,5 tons - 10 cbm,7 tons - 14 cbm";

    private static readonly string lclimport =
        "Country,Country Code,OriginCfs,OriginCfs Code,Origin Destination,OriginDestination Code,Origin Agent,Routing,Routing Code,MSRG w/m,MSRG min,Transit Time,Haz Surcharge,Network,Remark,Validity";

    private static readonly string fclimport =
        "SECTOR,POL,POLCODE,POD,PODCODE,20',40'STD,40'HC,CARRIER NAME,TRANSIT TIME, VALIDITY, REMARK";

    private static readonly string lclexport =
        "Preference,Port of loading,Origin Code,Dest Country,Dest CFS,Destination Code,SELL w/m,SELL min,Solas Fees,Routing,Routing Code,Service Ex-T/S Port,Freqcy Ex T/S Port,2nd T/S Port,1st Leg T/T,T/T Ex-1st T/s to 2nd T/S,T/T Ex 2nd T/S to F/Dstn,TTT,Unit,Frt Collect,HAZ cargo Aceptblty W/M,HAZ cargo Aceptblty MIN,WWA,Point of Custom Clearance,Validity";

    private static readonly string fclexportusa =
        "Pol,Pod,Total Freight 20',Total Freight 40',Total Freight 40HC,Remark,Pol Code,Pod Code,Carrier Name,Service ContractNo,Sales Person,Routing,Service,Sector,Transit Time,Effective Date,Validity,Commodity";

    private static readonly string fclexportnonusa =
        "Pol,Pod,Total Freight 20',Total Freight 40',Total Freight 40HC,Remark,Pol Code,Pod Code,Carrier Name,Service ContractNo,Sales Person,Routing,Service,Sector,Transit Time,Effective Date,Validity,Commodity";

    private static readonly string selllclexport =
        "Preference,Port of loading,Origin Code,Dest Country,Dest CFS,Destination Code,SELL w/m,SELL min,Solas Fees,Routing,Routing Code,Service Ex-T/S Port,Freqcy Ex T/S Port,2nd T/S Port,1st Leg T/T,T/T Ex-1st T/s to 2nd T/S,T/T Ex 2nd T/S to F/Dstn,TTT,Unit,Frt Collect,HAZ cargo Aceptblty W/M,HAZ cargo Aceptblty MIN,WWA,Point of Custom Clearance,Validity,Remark,THC per w/m,THC per ton,THC Min,THC Haz per w/m,THC Haz per Ton,THC Haz Min,DOC per HBL,DOC Haz per HBL,VGM,EBS,AMC,ENS,IHC";

    private static readonly string sellfclexportusa =
        "POL,POD,Total Freight 20',Total Freight 40', Total Freight 40HC,Sector,Commodity,Validity,Service,POL code, POD code,Carrier Name, Carrier Code,Terminal,Effective Date, Remark, THC 20',THC 40', THC 40HC,EBS 20',EBS 40',EBS 40HC,DOC 20',DOC 40',DOC 40HC,VGM 20',VGM 40',VGM 40HC,ENS 20',ENS 40',ENS 40HC,AMS 20',AMS 40',AMS 40HC,IHC 20',IHC 40',IHC 40HC,Preference,Service Contract, Sales, Routing";

    private static readonly string sellfclexportnonusa =
        "POL,POD,Total Freight 20',Total Freight 40',Total Freight 40HC,Sector,Commodity,Validity,Service,POL code,POD code,Carrier Name,Carrier Code,Terminal,Effective Date,Remark,THC 20',THC 40',THC 40HC,EBS 20',EBS 40',EBS 40HC,DOC 20',DOC 40',DOC 40HC,VGM 20',VGM 40',VGM 40HC,ENS 20',ENS 40',ENS 40HC,AMS 20',AMS 40',AMS 40HC,IHC 20',IHC 40',IHC 40HC,Preference,Service Contract,Sales,Routing";

    private static readonly string sealable = "Date,Shipping Bill Number,Destination,Label Count,User Id,User Email";

    public static string GetCSVDownloadString(UploadFileTypes uploadFileType)
    {
        return uploadFileType switch
        {
            UploadFileTypes.pincodes => pincodes,
            UploadFileTypes.agents => agents,
            UploadFileTypes.exworks => exworks,
            UploadFileTypes.postlanding => postlanding,
            UploadFileTypes.transportation => transportation,
            UploadFileTypes.buylclimports => lclimport,
            UploadFileTypes.buyfclimports => fclimport,
            UploadFileTypes.buylclexports => lclexport,
            UploadFileTypes.buyfclexportsusa => fclexportusa,
            UploadFileTypes.buyfclexportsnonusa => fclexportnonusa,
            UploadFileTypes.selllclexports => selllclexport,
            UploadFileTypes.sellfclexportsusa => sellfclexportusa,
            UploadFileTypes.sellfclexportsnonusa => sellfclexportnonusa,
            UploadFileTypes.sealable => sealable,
            _ => string.Empty
        };
    }
}
