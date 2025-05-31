using FluentValidation;
using CafeVirtual.Pruebas.API.Models;

namespace CafeVirtual.Pruebas.API.Validators
{
    public class ProductoViewModelValidator : AbstractValidator<ProductoViewModel>
    {
        public ProductoViewModelValidator()
        {
            RuleFor(x => x.IdProducto)
                .GreaterThan(0).WithMessage("Debe proporcionar un ID de producto válido.");

            RuleFor(x => x.IdProveedor)
                .GreaterThan(0).WithMessage("Debe proporcionar un ID de proveedor válido.");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del producto es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 50 caracteres.");

            RuleFor(x => x.Detalle)
                .NotEmpty().WithMessage("El detalle del producto es obligatorio.")
                .MaximumLength(300).WithMessage("El detalle no puede superar los 50 caracteres.");

            RuleFor(x => x.Precio)
                .GreaterThan(0).WithMessage("El precio debe ser mayor que 0.");

            RuleFor(x => x.Cantidad)
                .GreaterThan(0).WithMessage("La cantidad no puede ser negativa.");                
        }
    }
}
