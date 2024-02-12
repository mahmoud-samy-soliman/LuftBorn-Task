using FluentValidation;

namespace Takamol.Application.Clients.CreateClientCommand
{
    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        private const string ClientNameRequiredErrorMsg = "Client Name is required.";
        private const string ClientNameMaxLengthErrorMsg = "Client Name should not exceed 8 characters.";

        public CreateClientCommandValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty().WithMessage(ClientNameRequiredErrorMsg)
                .MaximumLength(8).WithMessage(ClientNameMaxLengthErrorMsg);
        }
    }
}
