using FluentValidation;
using Serverland.Data.Entities;

namespace Serverland.Data.DatabaseObjects;

public record ServerDto(int id, string model, int disk_count, string generation, double weight, bool os  ,int categoryId);

public record CreateServerDto(string model, int disk_count, string generation, double weight, bool os  ,int categoryId)
{
    public class CreateServerDtoValidator : AbstractValidator<CreateServerDto>
    {
        public CreateServerDtoValidator()
        {
            RuleFor(x => x.model).NotEmpty().Length(min: 0, max: 100);
            RuleFor(x => x.generation).NotEmpty().Length(min: 0, max: 10);
            RuleFor(x => x.categoryId).NotNull();
        }
    }
};
public record UpdatedServerDto(string model);
