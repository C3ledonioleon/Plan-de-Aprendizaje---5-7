using FluentValidation;
namespace sve.DTOs.Validations
{
    public class FuncionValidator : AbstractValidator<FuncionCreateDto>
    {
        public FuncionValidator()
        {
            RuleFor(f => f.IdEvento)
                .GreaterThan(0).WithMessage("El IdEvento debe ser mayor que 0");
            RuleFor(f => f.IdLocal)
                .GreaterThan(0).WithMessage("El IdLocal debe ser mayor que 0");
            RuleFor(f => f.FechaHora)
                .GreaterThan(DateTime.Now).WithMessage("La fecha debe ser futura");
        }
    }

    public class ActualizarFuncion : AbstractValidator<FuncionUpdateDto>
    {
        public ActualizarFuncion()
        {
            RuleFor(f => f.IdEvento)
                .GreaterThan(0).WithMessage("El IdSector debe ser mayor que 0");
            RuleFor(f => f.IdLocal)
                .GreaterThan(0).WithMessage("El IdSesion debe ser mayor que 0");
            RuleFor(f => f.FechaHora)
                .GreaterThan(DateTime.Now).WithMessage("La fecha debe ser futura");
        }
    }
    
}