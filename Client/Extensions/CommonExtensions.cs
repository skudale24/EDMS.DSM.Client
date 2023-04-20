namespace EDMS.DSM.Client.Extensions;

public static class FinancialExtensions
{
    public static string FormatNumber(string amount)
    {
        return Convert.ToDecimal(amount.Replace(",", "")).ToString("0.00");
    }

    public static string FormatAmount(string amount)
    {
        return Convert.ToDecimal(amount.Replace(",", "")).ToString("#,##0.00");
    }

    public static string UpdateRupee(string dollarAmount, decimal usdRate)
    {
        _ = decimal.TryParse(dollarAmount, out var amount);
        amount = Math.Ceiling(amount * usdRate);
        return amount.ToString("#,##0.00");
    }

    public static string GetTotal(decimal usd, params string[] values)
    {
        decimal total = 0;
        foreach (var value in values)
        {
            _ = decimal.TryParse(value, out var amount);
            if (usd != 0)
            {
                amount = Math.Round(amount * usd, MidpointRounding.AwayFromZero);
            }

            total += amount;
        }

        return Math.Round(total, MidpointRounding.AwayFromZero).ToString("0.00");
    }

    public static string GetTotalDollars(decimal usd, params string[] values)
    {
        decimal total = 0;
        foreach (var value in values)
        {
            _ = decimal.TryParse(value, out var amount);
            if (usd != 0)
            {
                amount = Math.Round(amount / usd, 2, MidpointRounding.AwayFromZero);
            }

            total += amount;
        }

        return Math.Round(total, 2, MidpointRounding.AwayFromZero).ToString("0.00");
    }
}
