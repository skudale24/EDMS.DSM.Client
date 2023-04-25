namespace EDMS.DSM.Client.Shared;

public partial class MainLayout : LayoutComponentBase
{
    private readonly TeamWorldTheme _theme = new();

    private bool _drawerOpen = true;
    private ErrorBoundary? _errorBoundary = new();
    private bool _isDarkMode;

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    protected override void OnInitialized()
    {
        StateHasChanged();
    }

    protected override void OnParametersSet()
    {
        _errorBoundary?.Recover();
    }
}
