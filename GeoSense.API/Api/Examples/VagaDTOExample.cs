using GeoSense.API.Application.DTOs.Vaga;
using Swashbuckle.AspNetCore.Filters;

namespace GeoSense.API.Api.Examples
{
    public class VagaDTOExample : IExamplesProvider<VagaDTO>
    {
        public VagaDTO GetExamples()
        {
            return new VagaDTO
            {
                Numero = 101,
                Tipo = 0,
                Status = 0,
                PatioId = 1
            };
        }
    }
}