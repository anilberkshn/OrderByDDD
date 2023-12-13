using FluentValidation;
using Infrastructure.Models.Request;

namespace Application.Validations
{
    public class GetAllValidation : AbstractValidator<GetAllDto>
    {
        public GetAllValidation()
        {
            RuleFor(x => x.Skip).NotEmpty().GreaterThanOrEqualTo(0)
                .WithMessage(" Skip cannot negative.");
            RuleFor(x => x.Take).NotEmpty().GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100).WithMessage("TooManyRequest or cannot negative.");
        }
    }
}