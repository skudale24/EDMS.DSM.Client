namespace EDMS.DSM.Client.DTO;

public class GridColumnDTO
{
    public string UniqueName { get; set; }
    public string HeaderText { get; set; }
    public string DataField { get; set; } // Additional property for mapping data fields
    public bool Visible { get; set; }
    public string ColumnType { get; set; }

    public GridColumnDTO(string uniqueName, string headerText, string dataField = "", bool visible = false, string columnType="")
    {
        UniqueName = uniqueName;
        HeaderText = headerText;
        DataField = dataField;
        Visible = visible;
        ColumnType = columnType;
    }
}
