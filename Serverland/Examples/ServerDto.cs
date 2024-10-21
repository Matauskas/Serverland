using Serverland.Data.DatabaseObjects;

namespace Serverland.Examples;
using Swashbuckle.AspNetCore.Filters;
public class ServerDtoExample: IExamplesProvider<List<ServerDto>>
{
    
    public List<ServerDto> GetExamples()
    {
        return new List<ServerDto>
        {
            new ServerDto( 1, "hpe" , 24, "11th",50.25,false,1),
        };
    }
}