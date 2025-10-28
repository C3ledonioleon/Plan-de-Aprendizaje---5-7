using FluentValidation;
using sve.Models;

namespace sve.Validations
{
    public class OrdenValidator : AbstractValidator<Orden>
    {
        public OrdenValidator()
        {
            RuleFor(o => o.IdTarifa)
                .GreaterThan(0).WithMessage("Debe seleccionar una tarifa válida.");

            RuleFor(o => o.IdCliente)
                .GreaterThan(0).WithMessage("Debe seleccionar un cliente válido.");

            RuleFor(o => o.Total)
                .GreaterThan(0).WithMessage("El total de la orden debe ser mayor que cero.");

            RuleFor(o => o.Fecha)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("La fecha de la orden no puede ser anterior a hoy.");

            RuleFor(o => o.Estado)
                .IsInEnum().WithMessage("El estado de la orden es inválido.");

            RuleFor(o => o.Entradas)
                .NotNull().WithMessage("Debe haber al menos una entrada.")
                .Must(list => list.Count > 0).WithMessage("Debe haber al menos una entrada en la orden.");

            // Validación condicional: si está pagada, total debe ser > 0
            RuleFor(o => o.Total)
                .GreaterThan(0)
                .When(o => o.Estado == EstadoOrden.Pagada)
                .WithMessage("Una orden pagada debe tener un total mayor a cero.");
        }
    }
}
