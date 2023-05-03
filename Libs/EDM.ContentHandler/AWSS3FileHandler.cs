using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Web.UI;

namespace EDM.ContentHandler
{
    public class AWSS3FileHandler : IFileHandler
    {
        #region --- Properties ---

        public String Module = String.Empty;
        public String Message = String.Empty;
        private string BucketPath { get; set; }
        private string AWS_AccessKeyId { get; set; }
        private string AWS_SecretAccessKeyId { get; set; }
        private string AWS_DefaultUploadLocation { get; set; }
        private string AWS_ReportLocation { get; set; }
        private string AWS_UploadFilePath { get; set; }
       // private string AWS_LPCRebatePDFPathPhysical { get; set; }
        private IAmazonS3 client;
        private Amazon.RegionEndpoint RegionEndpoint;
        #endregion --- Properties ---

        #region --- Constructors ---
        public AWSS3FileHandler(String ConfigKey)
        {
            AWS_AccessKeyId = EDM.Setting.DB.GetByName(EDM.Setting.Key.AWS_AccessKeyId,configKey: ConfigKey);
            AWS_SecretAccessKeyId = EDM.Setting.DB.GetByName(EDM.Setting.Key.AWS_SecretAccessKeyId, configKey: ConfigKey);
            AWS_DefaultUploadLocation = EDM.Setting.DB.GetByName(EDM.Setting.Key.AWS_DefaultUploadLocation, configKey: ConfigKey);
            AWS_ReportLocation = EDM.Setting.DB.GetByName(EDM.Setting.Key.AWS_ReportLocation, configKey: ConfigKey);
            AWS_UploadFilePath = EDM.Setting.DB.GetByName(EDM.Setting.Key.AWS_UploadFilePath, configKey: ConfigKey);
           // AWS_LPCRebatePDFPathPhysical = EDM.Setting.DB.GetByName(EDM.Setting.Key.AWS_LPCRebatePDFPathPhysical, configKey: ConfigKey);
            string val= EDM.Setting.DB.GetByName(EDM.Setting.Key.AWS_RegionEndpoint, configKey: ConfigKey);
            RegionEndpoint = (Amazon.RegionEndpoint) typeof(Amazon.RegionEndpoint).GetField(val, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).GetValue(null);
        }
        public AWSS3FileHandler(String module, FileLocationType fileLocationType,String configKey) : this(configKey)
        {
            Module = module;
            setPath(fileLocationType);
        }

        #endregion --- Constructors ---

        #region --- Public Methods ---
        public bool UploadFile(UploadedFile afile, string relLocation, string fileName)
        {
            try
            {
                relLocation = getAWSfilePathFormat(relLocation);

                TransferUtility fileTransferUtility = new
                                    TransferUtility(new AmazonS3Client(AWS_AccessKeyId, AWS_SecretAccessKeyId, RegionEndpoint));
                //The location should be like bucketname/folderName 
                //(e.g. tempedmsescoredev/UPLOAD/2017/10/1000234) remember that there is no slash (/) at the end of path
                string bucketName = BucketPath + relLocation;
                // 1. Upload a file, file name is used as the object key name.
                fileTransferUtility.Upload(afile.InputStream, bucketName, fileName);
                return true;
            }
            catch (AmazonS3Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.ContentHandler:AWSS3FileHandler", "UploadFile(UploadedFile,string,string)", ex);
                return false;
            }
        }
        public bool UploadFile(string sourceFilePath, string relLocation, string fileName)
        {
            try
            {
                relLocation = getAWSfilePathFormat(relLocation);

                TransferUtility fileTransferUtility = new
                                    TransferUtility(new AmazonS3Client(AWS_AccessKeyId, AWS_SecretAccessKeyId, RegionEndpoint));
                //The location should be like bucketname/folderName 
                //(e.g. tempedmsescoredev/UPLOAD/2017/10/1000234) remember that there is no slash (/) at the end of path
                string bucketName = BucketPath + relLocation;
                // 1. Upload a file, file name is used as the object key name.
                fileTransferUtility.Upload(sourceFilePath, bucketName, fileName);
                return true;
            }
            catch (AmazonS3Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.ContentHandler:AWSS3FileHandler", "UploadFile(string,string,string)", ex);
                return false;
            }
        }
        public byte[] GetFile(string filePath)
        {
            try
            {
                string keyName = Path.GetFileName(filePath);
                string relLocation = getAWSfilePathFormat(Path.GetDirectoryName(filePath));
                string bucketName = BucketPath + relLocation;

                return ReadObjectData(bucketName, keyName);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                if (ex.Message.Contains("404"))
                {
                    Common.Log.Info(Module, Module + ":EDM.ContentHandler:AWSS3FileHandler", "GetFile:" + Message);
                }
                else
                {
                    Common.Log.Error(Module, Module + ":EDM.ContentHandler:AWSS3FileHandler", "GetFile", ex);
                }
                return null;
            }
        }
        public bool DeleteFile(string filePath)
        {
            try
            {
                string keyName = Path.GetFileName(filePath);
                string relLocation = getAWSfilePathFormat(Path.GetDirectoryName(filePath));
                string bucketName = BucketPath + relLocation;

                return DeleteObjectData(bucketName, keyName);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                if (ex.Message.Contains("404"))
                {
                    Common.Log.Info(Module, Module + ":EDM.ContentHandler:AWSS3FileHandler", "DeleteFile:" + Message);
                }
                else
                {
                    Common.Log.Error(Module, Module + ":EDM.ContentHandler:AWSS3FileHandler", "DeleteFile", ex);
                }
                return false;
            }
        }
        public bool IsFileExists(string filePath)
        {
            try
            {
                string keyName = Path.GetFileName(filePath);
                string relLocation = getAWSfilePathFormat(Path.GetDirectoryName(filePath));
                string bucketName = BucketPath + relLocation;

                return IsObjectDataExist(bucketName, keyName);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                if (ex.Message.Contains("404"))
                {
                    Common.Log.Info(Module, Module + ":EDM.ContentHandler:AWSS3FileHandler", "IsFileExists:" + Message);
                }
                else
                {
                    Common.Log.Error(Module, Module + ":EDM.ContentHandler:AWSS3FileHandler", "IsFileExists", ex);
                }
                return false;
            }
        }

        public string getPhysicalPath(string relLocation)
        {
            return BucketPath+ getAWSfilePathFormat(relLocation);
        }
        #endregion --- Public Methods ---
        #region --- Private Methods ---
        private string getAWSfilePathFormat(string relLocation)
        {
            string strResult = string.Empty;
            try
            {
                strResult = relLocation.Replace("\\", "/").Replace("//", "/");
                if (strResult.IndexOf("/") != 0)
                {
                    strResult = "/" + strResult;
                }
                if (strResult.LastIndexOf("/") == strResult.Length - 1)
                {
                    strResult = strResult.Substring(0, strResult.Length - 1);
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.ContentHandler:AWSS3FileHandler", "getAWSfilePathFormat", ex);
            }
            return strResult;
        }

        private byte[] ReadObjectData(string bucketName, string keyName)
        {
            try
            {
                using (client = new AmazonS3Client(AWS_AccessKeyId, AWS_SecretAccessKeyId, RegionEndpoint))
                {
                    GetObjectRequest request = new GetObjectRequest
                    {
                        BucketName = bucketName,
                        Key = keyName
                    };
                    byte[] buffer = new byte[32 * 1024];
                    using (GetObjectResponse response = client.GetObject(request))
                    using (Stream responseStream = response.ResponseStream)
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        return ms.ToArray();
                    }
                }
            }
            catch (AmazonS3Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.ContentHandler:AWSS3FileHandler", "ReadObjectData", ex);
                return null;
            }
        }
        private bool DeleteObjectData(string bucketName, string keyName)
        {
            try
            {
                using (client = new AmazonS3Client(AWS_AccessKeyId, AWS_SecretAccessKeyId, RegionEndpoint))
                {
                    DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest
                    {
                        BucketName = bucketName,
                        Key = keyName
                    };
                    client.DeleteObject(deleteObjectRequest);
                    return true;
                }
            }
            catch (AmazonS3Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.ContentHandler:AWSS3FileHandler", "DeleteObjectData", ex);
                return false;
            }
        }
        private bool IsObjectDataExist(string bucketName, string keyName)
        {
            try
            {
                using (client = new AmazonS3Client(AWS_AccessKeyId, AWS_SecretAccessKeyId, RegionEndpoint))
                {
                    GetObjectRequest request = new GetObjectRequest
                    {
                        BucketName = bucketName,
                        Key = keyName
                    };
                    using (GetObjectResponse response = client.GetObject(request))
                        return true;
                }
            }
            catch (AmazonS3Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.ContentHandler:AWSS3FileHandler", "IsObjectDataExist", ex);
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;
            }
            return false;
        }
        private void setPath(FileLocationType fileLocationType)
        {
            try
            {
                switch(fileLocationType)
                {
                    case FileLocationType.None:
                    case FileLocationType.UploadFilePath:
                        BucketPath = AWS_UploadFilePath;
                        break;
                    case FileLocationType.DefaultUploadLocation:
                        BucketPath = AWS_DefaultUploadLocation;
                        break;
                    //case FileLocationType.LPCRebatePDFPathPhysical:
                    //    BucketPath = AWS_LPCRebatePDFPathPhysical;
                    //    break;
                    case FileLocationType.ReportLocation:
                        BucketPath = AWS_ReportLocation;
                        break;
                    default:
                        BucketPath = AWS_UploadFilePath;
                        break;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.ContentHandler:AWSS3FileHandler", "setPath", ex);
            }
        }
        #endregion --- Private Methods ---
    }
}
