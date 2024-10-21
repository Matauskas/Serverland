using Serverland.Data.DatabaseObjects;

namespace Serverland.Examples;
using Swashbuckle.AspNetCore.Filters;
public class ListPartDto: IExamplesProvider<List<PartDto>>
{
    
    public List<PartDto> GetExamples()
    {
        return new List<PartDto>
        {
            new PartDto( 1,"2 x intel xeon gold 6565", "32gb", "h740p","flr331","500GB ssd","2TB", "750W",false,1 ),
            new PartDto( 2, "2 x intel xeon gold 6565", "32gb", "h740p","flr331","500GB ssd","2TB", "750W",true,2 ),
        };
    }
}