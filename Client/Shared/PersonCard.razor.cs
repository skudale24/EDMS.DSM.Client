namespace EDMS.DSM.Client.Shared;

public partial class PersonCard
{
    [Parameter] public string? Class { get; set; }

    [Parameter] public string? Style { get; set; }

    [CascadingParameter] public string? LoggedUserName { get; set; }

    [Parameter] public string? Title { get; set; }
}
