using Serverland.Data.DatabaseObjects;

namespace Serverland.Examples;
using Swashbuckle.AspNetCore.Filters;
public class CreatePartDtoExample: IExamplesProvider<CreatePartDto>
{
    
    public CreatePartDto GetExamples()
    {

        return new CreatePartDto("intel xeon silver 5462", "1024gb", "30p","flr331","500GB ssd","2TB", "750W",true  ,1 );
    }
}