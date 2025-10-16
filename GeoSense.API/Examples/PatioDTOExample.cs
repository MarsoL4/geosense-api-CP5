using GeoSense.API.DTOs.Patio;
using Swashbuckle.AspNetCore.Filters;

namespace GeoSense.API.Examples
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