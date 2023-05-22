using EDMS.DSM.Server.Models;
using System.Collections.Generic;

namespace EDMS.DSM.Data
{
    public class GridData
    {
        public List<GridItemRequest> Communications { get; set; }
        public List<GridColumnRequest> GridColumns { get; set; }
    }
}
