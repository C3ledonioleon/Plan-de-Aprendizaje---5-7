using FluentValidation;
using sve.Models;

namespace sve.Validators
{
    public class EventoValidator : AbstractValidator<Evento>
    {
        public EventoValidator()
        {
            RuleFor(e => e.Nombre)
                .NotEmpty().WithMessage("El nombre del evento es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres.");

            RuleFor(e => e.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres.");

            RuleFor(e => e.FechaInicio)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("La fecha de inicio debe ser futura o actual.");

            RuleFor(e => e.FechaFin)
                .GreaterThan(e => e.FechaInicio).WithMessage("La fecha de fin debe ser posterior a la fecha de inicio.");

            RuleFor(e => e.Estado)
                .IsInEnum().WithMessage("El estado del evento no es válido.");
        }
    }
}
