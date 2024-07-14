namespace OpenMovies.WebApi.Validators;

public sealed class ProfileEditingValidator
    : AbstractValidator<ProfileEditingRequest>, IValidator<ProfileEditingRequest>
{
    public ProfileEditingValidator()
    {
        RuleFor(request => request.Name)
            .MinimumLength(3).WithMessage("Name must be at least 3 characters.")
            .NotEmpty().WithMessage("Name is required.");
    }
}