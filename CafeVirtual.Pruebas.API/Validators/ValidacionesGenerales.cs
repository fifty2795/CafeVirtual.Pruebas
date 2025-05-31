using FluentValidation;

namespace CafeVirtual.Pruebas.API.Validators
{
    public static class ValidacionesGenerales
    {
        public static IRuleBuilderOptions<T, string> Nombre<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no debe superar los 100 caracteres.");
        }

        public static IRuleBuilderOptions<T, string> EmailComun<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("Formato de correo inválido.")
                .MaximumLength(150).WithMessage("El correo no debe superar los 150 caracteres.");
        }     

        public static IRuleBuilderOptions<T, string> SoloNumerosEnteros<T>(this IRuleBuilder<T, string> ruleBuilder, string fieldName)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"El campo '{fieldName}' es obligatorio.")
                .Matches(@"^\d+$").WithMessage($"El campo '{fieldName}' solo permite números enteros.");
        }

        public static IRuleBuilderOptions<T, string> SoloNumerosDecimales<T>(this IRuleBuilder<T, string> ruleBuilder, string fieldName)
        {
            return ruleBuilder
              .NotEmpty().WithMessage($"El campo '{fieldName}' es obligatorio.")
              .Matches(@"^\d+(\.\d{1,2})?$").WithMessage($"El campo '{fieldName}' solo permite números enteros con hasta 2 decimales.");            
        }

        public static IRuleBuilderOptions<T, DateTime?> FechaInicioFin<T>(this IRuleBuilder<T, DateTime?> ruleBuilder, Func<T, DateTime?> fechaContrariaSelector, string mensajeAmbosRequeridos = "Ambas fechas deben ser ingresadas.",
        string mensajeRangoInvalido = "La fecha de inicio debe ser menor o igual a la fecha de fin.")
        {
            return ruleBuilder
                .Must((obj, fechaActual) =>
                {
                    var fechaContraria = fechaContrariaSelector(obj);
                    
                    if ((fechaActual.HasValue && !fechaContraria.HasValue) || (!fechaActual.HasValue && fechaContraria.HasValue))
                        return false;
                    
                    if (fechaActual.HasValue && fechaContraria.HasValue)
                    {
                        return fechaActual <= fechaContraria;
                    }

                    return true; 
                })
                .WithMessage((obj, fechaActual) =>
                {
                    var fechaContraria = fechaContrariaSelector(obj);

                    if ((fechaActual.HasValue && !fechaContraria.HasValue) ||
                        (!fechaActual.HasValue && fechaContraria.HasValue))
                        return mensajeAmbosRequeridos;

                    return mensajeRangoInvalido;
                });
        }
    }
}
