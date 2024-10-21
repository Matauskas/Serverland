using Serverland.Data.DatabaseObjects;
using Serverland.Data.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Serverland.Examples;

public class UpdatedServerDtoExample : IExamplesProvider<UpdatedServerDto>
{
    public UpdatedServerDto GetExamples()
    {
        return new UpdatedServerDto("example update");
    } 
}