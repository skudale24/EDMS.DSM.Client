using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDM.PDFMappingVariables
{
    public class TemplateField
    {
        public int FieldId { get; set; }
        public string ColumnName { get; set; }
        public string QueryColumn { get; set; }
        public string FieldType { get; set; }
    }
}
