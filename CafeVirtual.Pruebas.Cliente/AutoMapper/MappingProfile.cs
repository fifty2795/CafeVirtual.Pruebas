using AutoMapper;
using CafeVirtual.Pruebas.Cliente.Models;
using CafeVirtual.Pruebas.Data.Models;

namespace CafeVirtual.Pruebas.Cliente.Automapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // Mapear Rol
            CreateMap<RolViewModel, TblRol>().ReverseMap();

            // Mapeo de SubMenús
            CreateMap<TblSubMenu, SubMenuConfigViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdSubMenu))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.Icono, opt => opt.MapFrom(src => src.Icono))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Activo));

            // Mapeo de Menús
            CreateMap<TblMenu, MenuConfigViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdMenu))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.Icono, opt => opt.MapFrom(src => src.Icono))
                .ForMember(dest => dest.Orden, opt => opt.MapFrom(src => src.Orden))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Activo))
                .ForMember(dest => dest.SubMenus, opt => opt.MapFrom(src => src.TblSubMenus));

            // Mapear Menus
            CreateMap<TblMenu, MenuViewModel>()                
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdMenu));                        
        }
    }
}
