namespace OpenMovies.WebApi.Validators;

public sealed class AuthenticationCredentialsValidator :
    AbstractValidator<AuthenticationCredentials>, IValidator<AuthenticationCredentials>
{
    public AuthenticationCredentialsValidator()
    {
        RuleFor(credential => credential.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email address format.");

        RuleFor(credential => credential.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}