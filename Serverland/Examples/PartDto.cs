using Serverland.Data.DatabaseObjects;

namespace Serverland.Examples;
using Swashbuckle.AspNetCore.Filters;
public class PartDtoExample: IExamplesProvider<List<PartDto>>
{
    
    public List<PartDto> GetExamples()
    {
        return new List<PartDto>
        {
            new PartDto( 5,"2 x intel xeon gold 656", "512gb", "30p","flr331","500GB ssd","2TB", "750W",false,2 ),
        };
    }
}