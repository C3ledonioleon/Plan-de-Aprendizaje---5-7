using FluentValidation;
using sve.DTOs;
using sve.Models;

namespace sve.Validations
{
    public class TarifaCreateValidator : AbstractValidator<TarifaCreateDto>
    {
        public TarifaCreateValidator()
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
                .IsInEnum().WithMessage("El estado de la tarifa no es válido.");
        }
    }

    public class TarifaUpdateValidator : AbstractValidator<TarifaUpdateDto>
    {
        public TarifaUpdateValidator()
        {
            RuleFor(t => t.Precio)
                .GreaterThan(0).WithMessage("El precio debe ser mayor que cero.");

            RuleFor(t => t.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");

            RuleFor(t => t.Estado)
                .IsInEnum().WithMessage("El estado de la tarifa no es válido.");
        }
    }
}


