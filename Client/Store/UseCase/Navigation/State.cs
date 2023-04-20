namespace EDMS.DSM.Client.Store.UseCase.Navigation;

[FeatureState]
public class State
{
    public State()
    {
    }

    public State(List<NavMenuDto> navMenus)
    {
        NavMenus = navMenus;
    }

    public List<NavMenuDto> NavMenus { get; set; } = new();
}
