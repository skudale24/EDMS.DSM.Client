using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Telerik.Web.UI;
using VTI.Common;

namespace EDM.ContentHandler
{
    public class LocalFileHandler : IFileHandler
    {
        #region --- Properties ---

        public String Module = String.Empty;
        public String Message = String.Empty;
        private string PhysicalPath { get; set; }
        private string VirtualPath { get; set; }
        private string ImageUrl { get; set; }
        private string LPCRebatePDFPath { get; set; }
        private string DefaultUploadLocation { get; set; }
        private string ReportLocation { get; set; }
        private string UploadFilePath { get; set; }
       // private string LPCRebatePDFPathPhysical { get; set; }
        #endregion --- Properties ---

        #region --- Constructors ---
        public LocalFileHandler(String ConfigKey)
        {
            //Virtual
            ImageUrl = EDM.Setting.DB.GetByName(EDM.Setting.Key.ImageUrl, configKey: ConfigKey);
            LPCRebatePDFPath = EDM.Setting.DB.GetByName(EDM.Setting.Key.LPCRebatePDFPath, configKey: ConfigKey);
            //Physical
            DefaultUploadLocation = EDM.Setting.DB.GetByName(EDM.Setting.Key.DefaultUploadLocation, configKey: ConfigKey);
            ReportLocation = EDM.Setting.DB.GetByName(EDM.Setting.Key.ReportLocation, configKey: ConfigKey);
            UploadFilePath = EDM.Setting.DB.GetByName(EDM.Setting.Key.UploadFilePath, configKey: ConfigKey);
            // LPCRebatePDFPathPhysical = EDM.Setting.DB.GetByName(EDM.Setting.Key.LPCRebatePDFPathPhysical, configKey: ConfigKey);
        }
        public LocalFileHandler(String module, FileLocationType fileLocationType, String configKey) : this(configKey)
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
                relLocation = getLocalfilePathFormat(relLocation);
                string fileLocation = PhysicalPath + relLocation;    
                if (!IOUtils.DirExists(fileLocation)) IOUtils.CreateDir(fileLocation);

                afile.SaveAs(fileLocation + fileName);
                return true;
            }
            catch(Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.ContentHandler:LocalFileHandler", "UploadFile(UploadedFile,string,string)", ex);
                return false;
            }
        }
        public bool UploadFile(string sourceFilePath, string relLocation, string fileName)
        {
            try
            {
                relLocation = getLocalfilePathFormat(relLocation);

                string fileLocation = PhysicalPath + relLocation;
                if (!IOUtils.DirExists(fileLocation)) IOUtils.CreateDir(fileLocation);

                File.Copy(sourceFilePath, fileLocation + fileName, true);
                Common.Log.Info(Module, $"{Module}:EDM.ContentHandler:LocalFileHandler", $"Source: {sourceFilePath}");
                Common.Log.Info(Module, $"{Module}:EDM.ContentHandler:LocalFileHandler", $"Destination: {fileLocation}{fileName}");
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.ContentHandler:LocalFileHandler", "UploadFile(string,string,string)", ex);
                return false;
            }
        }
        public byte[] GetFile(string filePath)
        {
            try
            {
                filePath = filePath.Replace("\\", "/").Replace("//", "/");
                byte[] fileAsByteArray;
                using (var webClient = new WebClient())
                {
                    fileAsByteArray = webClient.DownloadData(VirtualPath + filePath);
                }
                return fileAsByteArray;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                if (ex.Message.Contains("404"))
                {
                    Common.Log.Info(Module, Module + ":EDM.ContentHandler:LocalFileHandler", "GetFile:"+ Message);
                }
                else
                {
                    Common.Log.Error(Module, Module + ":EDM.ContentHandler:LocalFileHandler", "GetFile", ex);
                }
                return null;
            }
        }
        public bool DeleteFile(string filePath)
        {
            try
            {
                filePath = getLocalfilePathFormat(filePath); 
                string fullFilePath = PhysicalPath + filePath;
                if (File.Exists(fullFilePath))
                {
                    File.Delete(fullFilePath);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                if (ex.Message.Contains("404"))
                {
                    Common.Log.Info(Module, Module + ":EDM.ContentHandler:LocalFileHandler", "DeleteFile:" + Message);
                }
                else
                {
                    Common.Log.Error(Module, Module + ":EDM.ContentHandler:LocalFileHandler", "DeleteFile", ex);
                }       
            }
            return false;
        }
        public bool IsFileExists(string filePath)
        {
            try
            {
                filePath = getLocalfilePathFormat(filePath);
                string fullFilePath = PhysicalPath + filePath;
                if (File.Exists(fullFilePath))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                if (ex.Message.Contains("404"))
                {
                    Common.Log.Info(Module, Module + ":EDM.ContentHandler:LocalFileHandler", "IsFileExists:" + Message);
                }
                else
                {
                    Common.Log.Error(Module, Module + ":EDM.ContentHandler:LocalFileHandler", "IsFileExists", ex);
                }
            }
            return false;
        }
        public string getPhysicalPath(string relLocation)
        {
            return PhysicalPath + getLocalfilePathFormat(relLocation);
        }
        #endregion --- Public Methods ---

        #region --- Private Methods ---
        private string getLocalfilePathFormat(string relLocation)
        {
            string strResult = string.Empty;
            try
            {
                if (relLocation.StartsWith("/")) relLocation = relLocation.Substring(1);
                strResult = relLocation.Replace("/", "\\");
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.ContentHandler:LocalFileHandler", "getLocalfilePathFormat", ex);
            }
            return strResult;
        }
        private void setPath(FileLocationType fileLocationType)
        {
            try
            {
                switch (fileLocationType)
                {
                    case FileLocationType.None:
                    case FileLocationType.UploadFilePath:
                        PhysicalPath = UploadFilePath;
                        VirtualPath= ImageUrl;
                        break;
                    case FileLocationType.DefaultUploadLocation:
                        PhysicalPath = DefaultUploadLocation;
                        VirtualPath = ImageUrl;
                        break;
                    //case FileLocationType.LPCRebatePDFPathPhysical:
                    //    PhysicalPath = LPCRebatePDFPathPhysical;
                    //    VirtualPath = LPCRebatePDFPath;
                    //    break;
                    case FileLocationType.ReportLocation:
                        PhysicalPath = ReportLocation;
                        VirtualPath = ImageUrl;
                        break;
                    default:
                        PhysicalPath = UploadFilePath;
                        VirtualPath = ImageUrl;
                        break;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.ContentHandler:LocalFileHandler", "setPath", ex);
            }
        }
        #endregion --- Private Methods ---
    }
}
