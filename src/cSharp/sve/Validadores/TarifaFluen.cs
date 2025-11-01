using FluentValidation;
namespace sve.DTOs.Validations
{
    public class TarifaValidator : AbstractValidator<TarifaCreateDto>
    {
        public TarifaValidator()
        {
            RuleFor(t => t.IdFuncion)
                .GreaterThan(0).WithMessage("Debe especificar una función válida.");

            RuleFor(t => t.IdSector)
                .GreaterThan(0).WithMessage("Debe especificar un sector válido.");

            RuleFor(t => t.Precio)
                .GreaterThan(0).WithMessage("El precio debe ser mayor que cero.");

            RuleFor(t => t.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");

            RuleFor(t => t.Estado)
                .IsInEnum().WithMessage("El estado debe pertenecer en el rango de opciones válidas.");
        }
    }

    public class ActualizarTarifa : AbstractValidator<TarifaUpdateDto>
    {
        public ActualizarTarifa()
        {
            RuleFor(t => t.Precio)
                .GreaterThan(0).WithMessage("El precio debe ser mayor que cero.");

            RuleFor(t => t.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");

            RuleFor(t => t.Estado)
                .IsInEnum().WithMessage("El estado debe pertenecer en el rango de opciones válidas.");
        }
    }
}


