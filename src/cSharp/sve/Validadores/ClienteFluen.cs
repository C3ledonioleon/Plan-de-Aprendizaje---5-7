using FluentValidation;
namespace sve.DTOs.Validations
{
    public class ClienteValidator : AbstractValidator<ClienteCreateDto>
    {
        public ClienteValidator()
        {
            RuleFor(c => c.DNI.ToString())
                .Length(8).WithMessage("El DNI debe tener 8 dígitos.");

            RuleFor(c => c.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.");

            RuleFor(c => c.Telefono)
                .Length(10).WithMessage("El teléfono debe tener 10 dígitos.");

            RuleFor(c => c.IdUsuario)
                .GreaterThan(0).WithMessage("Debe asociarse un usuario válido al cliente.");

        }
    }
    public class ActualizarCliente : AbstractValidator<ClienteUpdateDto>
    {
        public ActualizarCliente()
        {
            RuleFor(c => c.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.");

            RuleFor(c => c.Telefono)
                .Length(10).WithMessage("El teléfono debe tener 10 dígitos.");

            RuleFor(c => c.IdUsuario)
                .GreaterThan(0).WithMessage("Debe asociarse un usuario válido al cliente.");
        }
    }

}