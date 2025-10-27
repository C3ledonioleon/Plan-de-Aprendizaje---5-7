using FluentValidation;
using sve.Models;

namespace sve.Validators
{
    public class LocalValidator : AbstractValidator<Local>
    {
        public LocalValidator()
        {
            RuleFor(l => l.Nombre)
                .NotEmpty().WithMessage("El nombre del local es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres.");

            RuleFor(l => l.Direccion)
                .NotEmpty().WithMessage("La dirección es obligatoria.")
                .MaximumLength(255).WithMessage("La dirección no puede tener más de 255 caracteres.");

            RuleFor(l => l.CapacidadTotal)
                .GreaterThan(0).WithMessage("La capacidad total debe ser mayor a 0.");
        }
    }
}