namespace Serverland.Data.DatabaseObjects;

using FluentValidation;
using Serverland.Data.Entities;

public record CategoryDto(int id, string manifacturer, string serverType);

public record CreateCategoryDto(string manifacturer, string serverType)
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.manifacturer).NotEmpty().Length(min: 0, max: 100);
            RuleFor(x => x.serverType).NotEmpty().Length(min: 0, max: 100);
        }
    }
};
public record UpdatedCategoryDto(string manifacturer);