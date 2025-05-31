using AutoMapper;
using CafeVirtual.Pruebas.API.DTO;
using CafeVirtual.Pruebas.API.Models;
using CafeVirtual.Pruebas.API.Services.DTO;
using CafeVirtual.Pruebas.Data.Models;

namespace CafeVirtual.Pruebas.API.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Login
            CreateMap<LoginViewModel, LoginDTO>();                        
            
            //#region Usuario

            CreateMap<TblUsuario, UsuarioDTO>()
                    .ForMember(dest => dest.RoleNombre, opt => opt.MapFrom(src => src.IdRolNavigation != null ? src.IdRolNavigation.Nombre : string.Empty))
                    .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => $"{src.Nombre} {src.ApellidoPaterno} ".Trim()));

            //#endregion            

            //#region Producto

            CreateMap<ProductoViewModel, TblProducto>();

            CreateMap<TblProducto, ProductoDTO>()                
                .ForMember(dest => dest.NombreProveedor, opt => opt.MapFrom(src => src.IdProveedorNavigation != null ? src.IdProveedorNavigation.Nombre : string.Empty))
                .ForSourceMember(src => src.TblDetalleVenta, opt => opt.DoNotValidate());

            //#endregion             
        }
    }
}
