using FluentValidation;
using sve.Models;

namespace sve.Validations;

public class ClienteValidator : AbstractValidator<Cliente>
{
    public ClienteValidator()
    {
        RuleFor(c => c.DNI)
            .NotEmpty().WithMessage("El DNI es obligatorio.")
            .Length(7, 8).WithMessage("El DNI debe tener entre 7 y 8 dígitos.");

        RuleFor(c => c.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("El nombre solo puede contener letras y espacios.");

        RuleFor(c => c.Telefono)
            .NotEmpty().WithMessage("El teléfono es obligatorio.");

        RuleFor(c => c.IdUsuario)
            .GreaterThan(0).WithMessage("Debe asociarse un usuario válido al cliente.");

        // Validación opcional: si tiene entradas, que no estén vacías
        RuleFor(c => c.Entradas)
            .Must(e => e == null || e.Count > 0)
            .WithMessage("El cliente debe tener al menos una entrada si se asigna la lista.");

        // Validación opcional: si tiene órdenes, que no estén vacías
        RuleFor(c => c.Ordenes)
            .Must(o => o == null || o.Count > 0)
            .WithMessage("El cliente debe tener al menos una orden si se asigna la lista.");
    }
}
