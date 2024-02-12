using FluentValidation;

namespace Takamol.Application.Clients.UpdateClientCommand
{
    public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
    {
        private const string ClientNameRequiredErrorMsg = "Client Name is required.";
        private const string ClientNameMaxLengthErrorMsg = "Client Name should not exceed 8 characters.";

        public UpdateClientCommandValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty().WithMessage(ClientNameRequiredErrorMsg)
                .MaximumLength(8).WithMessage(ClientNameMaxLengthErrorMsg);
        }
    }
}
