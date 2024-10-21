using Serverland.Data.DatabaseObjects;
using Serverland.Data.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Serverland.Examples;

public class UpdatedCategoryDtoExample : IExamplesProvider<UpdatedCategoryDto>
{
    public UpdatedCategoryDto GetExamples()
    {
        return new UpdatedCategoryDto("example update");
    } 
}