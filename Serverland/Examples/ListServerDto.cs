using Serverland.Data.DatabaseObjects;

namespace Serverland.Examples;
using Swashbuckle.AspNetCore.Filters;
public class ListServerDto: IExamplesProvider<List<ServerDto>>
{
    
    public List<ServerDto> GetExamples()
    {
        return new List<ServerDto>
        {
            new ServerDto( 1, "ibm 4555" , 16, "10th",50.25,false,1),
            new ServerDto( 2, "Dell" , 16, "14th",45.25,true,2),
        };
    }
}