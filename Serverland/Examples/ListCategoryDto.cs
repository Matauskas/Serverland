using Serverland.Data.DatabaseObjects;

namespace Serverland.Examples;

using Serverland.Data.Entities;
using Swashbuckle.AspNetCore.Filters;
public class ListCategoryDto: IExamplesProvider<List<CategoryDto>>
{
    
    public List<CategoryDto> GetExamples()
    {
        return new List<CategoryDto>
        {
            new CategoryDto( 1,"IBM","Tower"),
            new CategoryDto( 2,"Dell","Tower"),
        };
    }
}