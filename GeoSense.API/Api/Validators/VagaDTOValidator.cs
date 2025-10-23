using FluentValidation;
using GeoSense.API.Application.DTOs.Vaga;

namespace GeoSense.API.Api.Validators
{
    public class VagaDTOValidator : AbstractValidator<VagaDTO>
    {
        public VagaDTOValidator()
        {
            RuleFor(x => x.Numero)
                .GreaterThan(0).WithMessage("Número da vaga deve ser maior que zero.");

            RuleFor(x => x.PatioId)
                .GreaterThan(0).WithMessage("PatioId deve ser maior que zero.");

            RuleFor(x => x.Tipo)
                .InclusiveBetween(0, 4).WithMessage("Tipo inválido.");

            RuleFor(x => x.Status)
                .InclusiveBetween(0, 1).WithMessage("Status inválido.");
        }
    }
}