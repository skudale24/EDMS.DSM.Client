namespace EDMS.DSM.Server.Models
{
    public class GenerateLetterRequest
    {
        public string TemplateFile { get; set; } = "";
        public string NewLocalPath { get; set; } = "";
        public int ProgramId { get; set; }
        public int TemplateID { get; set; } = 2;
        public int LPCID { get; set; } = 242;
        public int TemplateType { get; set; } = 1;
        public int GeneratedBy { get; set; }
        public int BatchId { get; set; }
    }
}
