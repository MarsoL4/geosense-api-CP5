using Swashbuckle.AspNetCore.Filters;
using GeoSense.API.Controllers;

namespace GeoSense.API.Examples
{
    /// <summary>
    /// Oferece um Exemplo pronto MotoId nos Parâmetros de VagaAggregateV2Controller.
    /// </summary>
    public class AlocarMotoRequestExample : IExamplesProvider<VagaAggregateV2Controller.AlocarMotoRequest>
    {
        public VagaAggregateV2Controller.AlocarMotoRequest GetExamples()
        {
            return new VagaAggregateV2Controller.AlocarMotoRequest
            {
                MotoId = 1
            };
        }
    }
}