using Serverland.Data.DatabaseObjects;

namespace Serverland.Examples;

using Serverland.Data.Entities;
using Swashbuckle.AspNetCore.Filters;
public class CategoryDtoExample: IExamplesProvider<List<CategoryDto>>
{
    
    public List<CategoryDto> GetExamples()
    {
        return new List<CategoryDto>
        {
            new CategoryDto( 1,"HPE","Tower"),
        };
    }
}