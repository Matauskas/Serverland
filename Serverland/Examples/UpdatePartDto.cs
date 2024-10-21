using Serverland.Data.DatabaseObjects;
using Serverland.Data.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Serverland.Examples;

public class UpdatedPartDtoExample : IExamplesProvider<UpdatedPartDto>
{
    public UpdatedPartDto GetExamples()
    {
        return new UpdatedPartDto("example update");
    } 
}