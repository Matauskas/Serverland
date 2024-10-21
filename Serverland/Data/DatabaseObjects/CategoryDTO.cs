using FluentValidation;
using Serverland.Data.Entities;

namespace Serverland.Data.DatabaseObjects;

public record CategoryDto(int id, string manifacturer, string serverType);

public record CreateCategoryDto( int id, string manifacturer, string serverType)
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.manifacturer).NotEmpty().Length(min: 0, max: 100);
            RuleFor(x => x.serverType).NotEmpty().Length(min: 0, max: 100);
            RuleFor(x => x.id).NotNull();
        }
    }
};
public record UpdatedCategoryDto(string description);