﻿using System.Text.Json.Serialization;

namespace EDMS.DSM.Server.Models
{
    public class DownloadExcelFileRequest
    {
        public string TemplateFile { get; set; } = "";
        public string TemplateName { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public string ActionText { get; set; } = "";
        public string? NewLocalPath { get; set; } = "";
        public int ProgramId { get; set; } = 2;
        public int TemplateID { get; set; } = 2;
        public int LPCID { get; set; } = 242;
        public int TemplateType { get; set; } = 1;
        public int? BatchId { get; set; }
    }
}
