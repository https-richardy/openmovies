namespace OpenMovies.WebApi.Validators;

public sealed class ProfileCreationValidator
    : AbstractValidator<ProfileCreationRequest>, IValidator<ProfileCreationRequest>
{
    public ProfileCreationValidator()
    {
        RuleFor(request => request.Name)
            .MinimumLength(3).WithMessage("Name must be at least 3 characters.")
            .NotEmpty();
    }
}