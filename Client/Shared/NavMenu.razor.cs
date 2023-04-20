using EDMS.DSM.Client.Managers.Menu;
using EDMS.DSM.Client.Store.UseCase.Navigation;
using Action = EDMS.DSM.Client.Store.UseCase.Navigation.Action;

namespace EDMS.DSM.Client.Shared;

public partial class NavMenu : ComponentBase, IDisposable
{
    [Inject] private IDialogService _dialogService { get; set; } = default!;

    [Inject] private HttpInterceptorService _interceptor { get; set; } = default!;

    [Inject] private NavigationManager _navigationManager { get; set; } = default!;

    [Inject] public INavManager NavManager { get; set; } = default!;

    [Inject] private IState<State> NavState { get; set; } = default!;

    [Inject] public IDispatcher Dispatcher { get; set; } = default!;

    [Inject] private ILocalStorageService _localStorageService { get; set; } = default!;

    [Inject] private CustomAuthenticationStateProvider _authStateProvider { get; set; } = default!;

    private string LoggedUserName { get; set; } = string.Empty;
    private string VersionNo { get; set; } = string.Empty;


    private List<NavMenuDto> NavMenus { get; set; } = new();

    private string AlertAssignmentIcon { get; set; } =
        "<path d=\"M19,3A2,2 0 0,1 21,5V19A2,2 0 0,1 19,21H5A2,2 0 0,1 3,19V5A2,2 0 0,1 5,3H9.18C9.6,1.84 10.7,1 12,1C13.3,1 14.4,1.84 14.82,3H19M12,3A1,1 0 0,0 11,4A1,1 0 0,0 12,5A1,1 0 0,0 13,4A1,1 0 0,0 12,3M7,7V5H5V19H19V5H17V7H7M11,9H13V13.5H11V9M11,15H13V17H11V15Z\" />";

    public void Dispose()
    {
        _interceptor.DisposeEvent();
    }


    protected override void OnInitialized()
    {
        _interceptor.RegisterEvent();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Action action = new() { NavMenus = (await NavManager.GetMenus().ConfigureAwait(false)).Result };
            Dispatcher.Dispatch(action);
            NavMenus = NavState.Value.NavMenus;

            // select all the permission for Menus
            var claims = NavState.Value.NavMenus.SelectMany(n => n.Permissions).Select(x => x.PermissionCode).Distinct()
                .Concat(NavState.Value.NavMenus.SelectMany(n => n.Submenus).SelectMany(n => n.Permissions)
                    .Select(x => x.PermissionCode).Distinct());


            var LatestVersion = string.Empty;

            var userdata = (await NavManager.GetUserData().ConfigureAwait(false)).Result;
            if (userdata != null)
            {
                LoggedUserName = userdata.UserName;
                LatestVersion = userdata.LatestVersion;

                _authStateProvider.UpdateAuthenticationState(claims, userdata);
            }


            // select all the permission for SubMenus
            //_authStateProvider.UpdateAuthenticationState(NavState.Value.NavMenus.SelectMany(n => n.Submenus).SelectMany(n => n.Permissions).Select(x => x.PermissionCode).Distinct());


            VersionNo = EndPoints.Version;
            //Commenting the Manual version check
            // if (!VersionNo.Equals(LatestVersion)) OpenDialog(LatestVersion);

            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
    }

    public void OpenDialog(string latestVersion)
    {
        DialogParameters parameters = new() { ["LatestVersion"] = latestVersion };

        DialogOptions options = new()
        {
            CloseButton = false,
            MaxWidth = MaxWidth.Small,
            Position = DialogPosition.TopCenter,
            DisableBackdropClick = true,
            CloseOnEscapeKey = false
        };
        _ = _dialogService.Show<VersionAlertDialog>("Version Check", parameters, options);
    }

    private async Task Logout()
    {
        await _localStorageService.RemoveItemAsync(StorageConstants.UserToken).ConfigureAwait(false);
        _navigationManager.NavigateTo("/logout");
    }

    private string GetIcon(string icon)
    {
        return !string.IsNullOrWhiteSpace(icon)
            ? icon switch
            {
                "Photo" => Icons.Material.Outlined.Photo,
                "Photos" => Icons.Material.Outlined.PhotoLibrary,
                "AddAPhoto" => Icons.Material.Outlined.AddAPhoto,
                "AddRoad" => Icons.Material.Outlined.AddRoad,
                "ManageSearch" => Icons.Material.Outlined.ManageSearch,
                "Schedule" => Icons.Material.Outlined.Schedule,
                "TrackChanges" => Icons.Material.Outlined.TrackChanges,
                "Agent" => Icons.Material.Outlined.SupportAgent,
                "Calcualtor" => Icons.Material.Outlined.Calculate,
                "Home" => Icons.Material.Outlined.Home,
                "CurrencyRupee" => Icons.Material.Outlined.CurrencyRupee,
                "fa-calculator" => Icons.Material.Outlined.Calculate,
                "fa-image" => Icons.Material.Outlined.Photo,
                "AdRoad" => Icons.Material.Outlined.AddRoad,
                "BroadcastOnPersonal" => Icons.Material.Outlined.BroadcastOnPersonal,
                "PriceChange" => Icons.Material.Outlined.PriceChange,
                "CorporateFare" => Icons.Material.Outlined.CorporateFare,
                "SavedSearch" => Icons.Material.Outlined.SavedSearch,
                "Feedback" => Icons.Material.Outlined.Feedback,
                "Assignment" => Icons.Material.Outlined.Assignment,
                "Slideshow" => Icons.Material.Outlined.Slideshow,
                "DocumentScanner" => Icons.Material.Outlined.DocumentScanner,
                "Print" => Icons.Material.Outlined.Print,
                "Whatsapp" => Icons.Material.Outlined.Whatsapp,
                "Label" => Icons.Material.Outlined.Label,
                "AirLabel" => Icons.Material.Outlined.Airlines,
                "SeaLabel" => Icons.Material.Outlined.DirectionsBoat,
                "RequestQuote" => Icons.Material.Outlined.RequestQuote,
                _ => Icons.Material.Outlined.Person
            }
            : Icons.Material.Outlined.Person;
    }
}
