using FluentValidation;
namespace sve.DTOs.Validations
{
    public class SectorValidator : AbstractValidator<SectorCreateDto>
    {
        public SectorValidator()
        {
            RuleFor(s => s.Nombre)
                .NotEmpty().WithMessage("El nombre del sector es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre del sector debe tener al menos 3 caracteres.")
                .MaximumLength(100).WithMessage("El nombre del sector no puede superar los 100 caracteres.");

            RuleFor(s => s.Capacidad)
                .GreaterThan(0).WithMessage("La capacidad debe ser mayor que cero.");

            RuleFor(s => s.IdLocal)
                .GreaterThan(0).WithMessage("Debe especificar un ID de local válido.");
        }
    }

    public class ActualizarSector : AbstractValidator<SectorUpdateDto>
    {
        public ActualizarSector()
        {
            RuleFor(s => s.Nombre)
                .NotEmpty().WithMessage("El nombre del sector es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre del sector debe tener al menos 3 caracteres.")
                .MaximumLength(100).WithMessage("El nombre del sector no puede superar los 100 caracteres.");

            RuleFor(s => s.Capacidad)
                .GreaterThan(0).WithMessage("La capacidad debe ser mayor que cero.");

            RuleFor(s => s.IdLocal)
                .GreaterThan(0).WithMessage("Debe especificar un ID de local válido.");
        }
    }
}
