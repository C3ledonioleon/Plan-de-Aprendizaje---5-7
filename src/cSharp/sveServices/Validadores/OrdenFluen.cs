using FluentValidation;
using sveCore.DTOs;
namespace sveServicio.Validation
{
    public class OrdenValidator : AbstractValidator<OrdenCreateDto>
    {
        public OrdenValidator()
        {
              RuleFor(o => o.IdTarifa)
                .GreaterThan(0).WithMessage("El IdCliente debe ser mayor a 0");
            RuleFor(o => o.IdCliente)
                .GreaterThan(0).WithMessage("El IdSesion debe ser mayor a 0");
            RuleFor(o => o.Total)
                 .GreaterThan(0).WithMessage("El Total debe ser mayor a 0");
            RuleFor(o => o.Fecha)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de la orden no puede ser futura");
        }
    }
}
