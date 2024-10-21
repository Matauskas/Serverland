using Serverland.Data.DatabaseObjects;

namespace Serverland.Examples;
using Swashbuckle.AspNetCore.Filters;
public class CreateServerDtoExample: IExamplesProvider<CreateServerDto>
{
    
    public CreateServerDto GetExamples()
    {
        return new CreateServerDto("DELL r730xd" , 8, "13th",50.25,true,1);
    }
}