using FluentValidation;
namespace sve.DTOs.Validations
{
    public class LocalValidator : AbstractValidator<LocalCreateDto>
    {
        public LocalValidator()
        {
            RuleFor(l => l.Nombre)
                .NotEmpty().WithMessage("El nombre del local es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres.");

            RuleFor(l => l.Direccion)
                .NotEmpty().WithMessage("La dirección es obligatoria.")
                .MaximumLength(60).WithMessage("La dirección no puede tener más de 60 caracteres.");

            RuleFor(l => l.CapacidadTotal)
                .GreaterThan(0).WithMessage("La capacidad total debe ser mayor a 0.");
        }
    }

    public class ActualizarLocal : AbstractValidator<LocalUpdateDto>
    {
        public ActualizarLocal()
        {
            RuleFor(l => l.Nombre)
                .NotEmpty().WithMessage("El nombre del local es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres.");

            RuleFor(l => l.Direccion)
                .NotEmpty().WithMessage("La dirección es obligatoria.")
                .MaximumLength(60).WithMessage("La dirección no puede tener más de 60 caracteres.");

            RuleFor(l => l.CapacidadTotal)
                .GreaterThan(0).WithMessage("La capacidad total debe ser mayor a 0.");
        }
    }
}