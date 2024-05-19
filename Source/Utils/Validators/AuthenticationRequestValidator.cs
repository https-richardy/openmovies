namespace OpenMovies.WebApi.Validators;

public sealed class AuthenticationRequestValidator : AbstractValidator<AuthenticationRequest>, IValidator<AuthenticationRequest>
{
    public AuthenticationRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("The email cannot be empty.")
            .EmailAddress().WithMessage("The email must be a valid email address.");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("The password cannot be empty.");
    }
}