using EDMS.DSM.Client.Managers.Menu;
using EDMS.DSM.Client.Managers.User;

namespace EDMS.DSM.Client.Shared;

public partial class Logout : ComponentBase
{
    [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    [Inject] public INavManager NavManager { get; set; } = default!;

    [Inject] private IUserManager UserManager { get; set; } = default!;

    [Inject] private CustomAuthenticationStateProvider AuthStateProvider { get; set; } = default!;


    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync().ConfigureAwait(false);

        await AuthStateProvider.UpdateAuthenticationStateAsync(string.Empty, string.Empty).ConfigureAwait(false);
        NavigationManager.NavigateTo("/login");
    }
}
