using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDM.ContentHandler
{
    public interface IFileHandler
    {
        bool UploadFile(global::Telerik.Web.UI.UploadedFile afile, string relLocation, string fileName);
        bool UploadFile(string sourceFilePath, string relLocation, string fileName);
        byte[] GetFile(string filePath);
        bool DeleteFile(string filePath);
        bool IsFileExists(string filePath);
        string getPhysicalPath(string relLocation);
    }
}
