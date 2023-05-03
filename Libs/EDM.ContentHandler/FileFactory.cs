using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDM.ContentHandler
{
    public abstract class FileFactory
    {
        public String Module = String.Empty;
        public String ConfigKey = String.Empty;
        public String Message = String.Empty;
        public FileLocationType fileLocationType;

        public IFileHandler GetFileUploadInstance(long DocTypeId, out string Storage)
        {
            return upload(DocTypeId,out Storage);
        }
        public IFileHandler GetFileDownloadInstance(string Storage)
        {
            return download(Storage);
        }
        public IFileHandler GetFileDeleteInstance(string Storage)
        {
            return delete(Storage);
        }

        protected abstract IFileHandler upload(long DocTypeId,out string Storage);
        protected abstract IFileHandler download(string Storage);
        protected abstract IFileHandler delete(string Storage);
    }
}
