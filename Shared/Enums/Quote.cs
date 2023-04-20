namespace EDMS.Shared.Enums;

public enum WeightType
{
    Lbs,
    Kgs
}

public enum QuoteType
{
    EXWORK,
    DAP,
    DDP,
    DDU
}

public enum QuoteSource
{
    M,
    W
}

//'A = Active, P = Inprocess, C = Closed, E = Executed, F = Failed
public enum QuoteStatus
{
    Active,
    Inprocess,
    Closed,
    Executed,
    Failed
}
