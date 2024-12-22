using FluentValidation;
using KKApi.Models.DTOs;

namespace KKApi.Validators
{
    public class QuoteRequestDtoValidator : AbstractValidator<QuoteRequestDto>
    {
        public QuoteRequestDtoValidator()
        {
            RuleFor(x => x.Topics)
                .NotNull().WithMessage("Topics cannot be null.")
                .NotEmpty().WithMessage("Topics cannot be empty.")
                .Must(topics => topics.Values.All(value => value >= 0)).WithMessage("All topic values must be greater than or equal to 0.");
        }
    }
}
