using FluentValidation;

namespace EDMS.DSM.Client.Validators;

public class ExWorksQuoteRequestFluentValidator : AbstractValidator<QuoteRequest>
{
    public ExWorksQuoteRequestFluentValidator()
    {
        When(x => x.ValidationEnabled, () =>
        {
            _ = RuleFor(x => x.ToPincode)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Please enter an valid Pincode");
            //.MustAsync(async (value, cancellationToken) => await IsValidPincodeAsync(value));

            When(x => x.IsWeightInLBs, () =>
            {
                _ = RuleFor(x => x.Weight)
                    .GreaterThan(0).WithMessage("Weight should be more than 0 lbs")
                    .LessThan(11000).WithMessage("Weight should be less than 11000 lbs");
            }).Otherwise(() =>
            {
                _ = RuleFor(x => x.Weight)
                    .GreaterThan(0).WithMessage("Weight should be more than 0 kgs")
                    .LessThan(5000).WithMessage("Weight should be less than 5000 kgs");
            });

            _ = RuleFor(x => x.Cbm)
                .GreaterThan(0).WithMessage("CBM should be more than 0")
                .LessThan(15).WithMessage("CBM should be less than 15");
        });
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result =
            await ValidateAsync(ValidationContext<QuoteRequest>.CreateWithOptions((QuoteRequest)model,
                x => x.IncludeProperties(propertyName))).ConfigureAwait(false);
        return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
    };

    private async Task<bool> IsValidPincodeAsync(string toPincode)
    {
        // Simulates a long running http call
        return toPincode.ToLower() != "000000";
    }
}
