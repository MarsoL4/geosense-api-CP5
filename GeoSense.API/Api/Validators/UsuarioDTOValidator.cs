using FluentValidation;
using GeoSense.API.Application.DTOs.Usuario;

namespace GeoSense.API.Api.Validators
{
    public class UsuarioDTOValidator : AbstractValidator<UsuarioDTO>
    {
        public UsuarioDTOValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório.")
                .EmailAddress().WithMessage("Email em formato inválido.")
                .MaximumLength(100).WithMessage("Email deve ter no máximo 100 caracteres.");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória.")
                .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres.");

            RuleFor(x => x.Tipo)
                .InclusiveBetween(0, 1).WithMessage("Tipo inválido.");
        }
    }
}