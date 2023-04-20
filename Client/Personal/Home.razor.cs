namespace EDMS.DSM.Client.Personal;

public partial class Home : ComponentBase, IDisposable
{
    private readonly bool _arrows = false;
    private readonly bool _autocycle = true;
    private readonly bool _bullets = false;
    private MudCarousel<string> _carousel;
    private readonly IList<string> lstbanners = new List<string>();
    private int selectedIndex = 1;


    [Inject] private HttpInterceptorService _interceptor { get; set; } = default!;

    public void Dispose()
    {
        _interceptor.DisposeEvent();
    }

    protected override async Task OnInitializedAsync()
    {
        base.OnInitialized();
        _interceptor.RegisterEvent();
    }
}
