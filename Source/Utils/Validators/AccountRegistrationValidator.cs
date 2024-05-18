namespace OpenMovies.WebApi.Validators;

public sealed class AccountRegistrationValidator : AbstractValidator<AccountRegistrationRequest>, IValidator<AccountRegistrationRequest>
{
    public AccountRegistrationValidator()
    {
        RuleFor(request => request.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
            .MaximumLength(20).WithMessage("Username cannot exceed 20 characters.")
            .Matches("^[a-zA-Z0-9_-]+$").WithMessage("Username can only contain letters, numbers, underscores (_), and hyphens (-).")
            .Must(username => !username.Contains(" ")).WithMessage("Username cannot contain spaces.");

        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be valid.");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.");
    }
}