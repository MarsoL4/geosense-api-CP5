using GeoSense.API.Application.DTOs.Patio;
using Swashbuckle.AspNetCore.Filters;

namespace GeoSense.API.Api.Examples
{
    public class PatioDTOExample : IExamplesProvider<PatioDTO>
    {
        public PatioDTO GetExamples()
        {
            return new PatioDTO
            {
                Nome = "Pátio Central"
            };
        }
    }
}