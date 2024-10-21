using FluentValidation;
using Serverland.Data.Entities;

namespace Serverland.Data.DatabaseObjects;

public record PartDto(int id, string cpu, string ram, string raid, string network, string ssd, string hdd, string psu, bool rails, int serverId);

public record CreatePartDto( int id, string cpu, string ram, string raid, string network, string ssd, string hdd, string psu, bool rails, int serverId)
{
    public class CreatePartDtoValidator : AbstractValidator<CreatePartDto>
    {
        public CreatePartDtoValidator()
        {
            RuleFor(x => x.cpu).NotEmpty().Length(min: 0, max: 100);
            RuleFor(x => x.ram).NotEmpty().Length(min: 0, max: 100);
            RuleFor(x => x.raid).NotEmpty().Length(min: 0, max: 100);
            RuleFor(x => x.network).NotEmpty().Length(min: 0, max: 100);
            RuleFor(x => x.psu).NotEmpty().Length(min: 0, max: 15);
            RuleFor(x => x.serverId).NotNull();
            RuleFor(x => x.id).NotNull();
        }
    }
};
public record UpdatedPartDto(string description);