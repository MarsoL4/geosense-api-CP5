using GeoSense.API.DTOs.Moto;
using Swashbuckle.AspNetCore.Filters;

namespace GeoSense.API.Examples
{
    public class MotoDTOExample : IExamplesProvider<MotoDTO>
    {
        public MotoDTO GetExamples()
        {
            return new MotoDTO
            {
                Modelo = "Honda CG 160",
                Placa = "ABC1D23",
                Chassi = "9C2JC4110JR000001",
                ProblemaIdentificado = "Motor com ruído excessivo",
                VagaId = 1
            };
        }
    }
}