namespace EDMS.DSM.Client.DTO
{
    public class GenerateLetterDTO
    {
        public string TemplateFile { get; set; } = default!;
        public string NewLocalPath { get; set; } = default!;
        public int ProgramId { get; set; }
        public int TemplateID { get; set; }
        public int LPCID { get; set; }
        public int TemplateType { get; set; }
        public int GeneratedBy { get; set; }
        public string? TemplateName { get; set; }
        public string? GeneratedFilePath { get; set; }
        public int BatchId { get; set; }
    }
}
