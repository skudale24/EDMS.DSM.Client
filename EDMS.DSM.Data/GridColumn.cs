namespace EDMS.DSM.Data
{
    public class GridColumnRequest
    {
        public string UniqueName { get; set; }
        public string HeaderText { get; set; }
        public string DataField { get; set; }
        public bool Visible { get; set; } = true;
        public string ColumnType { get; set; }
    }
}
