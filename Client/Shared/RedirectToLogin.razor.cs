namespace EDMS.DSM.Client.Shared;

public partial class RedirectToLogin
{
    [Inject] private NavigationManager NavManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        //The sole purpose of this Razor Component is to immediately redirect the user to the login page.
        NavManager.NavigateTo("/login");
    }
}
