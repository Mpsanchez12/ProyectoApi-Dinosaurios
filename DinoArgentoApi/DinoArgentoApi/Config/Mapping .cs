using System.Data;
using AutoMapper;
using DinoArgentoApi.Models.Dieta;
using DinoArgentoApi.Models.Dieta.Dto;
using DinoArgentoApi.Models.Dinosaurio;
using DinoArgentoApi.Models.Dinosaurio.Dto;
using DinoArgentoApi.Models.Periodo;
using DinoArgentoApi.Models.Periodo.Dto;
using DinoArgentoApi.Models.Role;
using DinoArgentoApi.Models.Role.Dto;
using DinoArgentoApi.Models.User;
using DinoArgentoApi.Models.User.Dto;

namespace DinoArgentoApi.Config
{
    public class Mapping: Profile
    {
        public Mapping()
        {

            CreateMap<Dieta, DietaDTO>().ReverseMap();
            CreateMap<Dieta, CreateDietaDTO>().ReverseMap();


            CreateMap<Periodo, PeriodoDTO>().ReverseMap();


            CreateMap<CreateDinosaurioDTO, Dinosaurio>();
            CreateMap<Dinosaurio, DinosaurioDTO>()
                .ForMember(dest => dest.Dietas, opt => opt.MapFrom(src => src.Dietas.Select(d => d.Nombre)))
                .ForMember(dest => dest.Periodo, opt => opt.MapFrom(src => 
                src.PeriodoId ==1 ? "Tri+asico":
                src.PeriodoId == 2 ? "Jurásico":
                src.PeriodoId == 3 ? "Cretácico": "Periodo no encontrado"));
            CreateMap<UpdateDinosaurioDTO, Dinosaurio>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Name)));
            CreateMap<RegisterDTO, User>();


            CreateMap<Role, RoleDTO>().ReverseMap();
        }
    }
}
    
