using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDM.ContentHandler
{
    public class FileHandlerCreator : FileFactory
    {
        #region --- Constructors ---
        public FileHandlerCreator(String module, FileLocationType? fileLocationType = null, String configKey ="")
        {
            Module = module;
            ConfigKey = configKey;
            if (fileLocationType.HasValue)
            {
                this.fileLocationType = fileLocationType.Value;
            }
            else
            {
                this.fileLocationType = FileLocationType.None;
            }
        }
        #endregion --- Constructors ---

        #region --- Protected Methods ---
        protected override IFileHandler upload(long DocTypeId,out string Storage)
        {
            DocTypeStorage objDocTypeStorage = new DocTypeStorage(Module, ConfigKey);
            objDocTypeStorage.DocTypeId = DocTypeId;
            objDocTypeStorage.GetById();
            Storage = objDocTypeStorage.Storage;

            return GetInstance(Storage);
        }
        protected override IFileHandler download(string Storage)
        {
            return GetInstance(Storage);
        }
        protected override IFileHandler delete(string Storage)
        {
            return GetInstance(Storage);
        }
        #endregion --- Protected Methods ---

        #region --- Private Methods ---
        private IFileHandler GetInstance(string Storage)
        {
            IFileHandler resultObj = null;
            switch (Storage)
            {
                case "AWS":
                    resultObj= new AWSS3FileHandler(Module, fileLocationType, ConfigKey);
                    break;
                case "Local":
                    resultObj= new LocalFileHandler(Module, fileLocationType, ConfigKey);
                    break;
                default:
                    resultObj= new LocalFileHandler(Module, fileLocationType, ConfigKey);
                    break;
            }
            return resultObj;
        }
        #endregion --- Private Methods ---
    }
}
