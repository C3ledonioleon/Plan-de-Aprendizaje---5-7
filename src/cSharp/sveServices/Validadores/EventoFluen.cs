using FluentValidation;
using sveCore.DTOs;
namespace sveServicio.Validation
{
    public class EventoValidator : AbstractValidator<EventoCreateDto>
    {
        public EventoValidator()
        {
            RuleFor(e => e.Nombre)
                .NotEmpty().WithMessage("El nombre del evento es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres.");

            RuleFor(e => e.Descripcion)
                .MaximumLength(200).WithMessage("La descripción no puede superar los 200 caracteres.");

            RuleFor(e => e.FechaInicio)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("La fecha de inicio debe ser futura o actual.");

            RuleFor(e => e.FechaFin)
                .GreaterThan(e => e.FechaInicio).WithMessage("La fecha de fin debe ser posterior a la fecha de inicio.");

            RuleFor(e => e.Estado)
                .IsInEnum().WithMessage("El estado del evento no es válido.");
        }
    }

    public class ActualizarEvento : AbstractValidator<EventoUpdateDto>
    {
        public ActualizarEvento()
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