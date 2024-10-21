using Serverland.Data.DatabaseObjects;

namespace Serverland.Examples;

using Serverland.Data.Entities;
using Swashbuckle.AspNetCore.Filters;
public class CreateCategoryDtoExample: IExamplesProvider<CreateCategoryDto>
{
    
    public CreateCategoryDto GetExamples()
    {

        return new CreateCategoryDto("HPE","Tower");
    }
}