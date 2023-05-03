using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDM.PDFMappingVariables
{
    public class TemplateMapping
    {
        public int MappingId { get; set; }
        public string TemplateVariable { get; set; }
        public string FieldExpression { get; set; }
        public string FieldExpressionValue { get; set; }
        public string ExpressionType { get; set; }
    }
}
