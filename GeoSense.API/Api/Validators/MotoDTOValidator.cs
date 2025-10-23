using FluentValidation;
using GeoSense.API.Application.DTOs.Moto;

namespace GeoSense.API.Api.Validators
{
    public class MotoDTOValidator : AbstractValidator<MotoDTO>
    {
        public MotoDTOValidator()
        {
            RuleFor(x => x.Modelo)
                .NotEmpty().WithMessage("Modelo é obrigatório.")
                .MaximumLength(50).WithMessage("Modelo deve ter no máximo 50 caracteres.");

            RuleFor(x => x.Placa)
                .NotEmpty().WithMessage("Placa é obrigatória.")
                .Matches(@"^[A-Z0-9-]{1,10}$").WithMessage("Placa inválida. Use letras maiúsculas, números e '-' (até 10 chars).");

            RuleFor(x => x.Chassi)
                .NotEmpty().WithMessage("Chassi é obrigatório.")
                .MaximumLength(50).WithMessage("Chassi deve ter no máximo 50 caracteres.");

            RuleFor(x => x.VagaId)
                .GreaterThan(0).WithMessage("VagaId deve ser maior que zero.");
        }
    }
}