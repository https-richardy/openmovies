namespace OpenMovies.WebApi.Validators;

public sealed class ProfileCreationValidator
    : AbstractValidator<ProfileCreationRequest>, IValidator<ProfileCreationRequest>
{
    public ProfileCreationValidator()
    {
        RuleFor(profile => profile.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters.");
    }
}