using AutoMapper;
using Demo.BusinessLogic.DTOS.EmployeeDTOS;
using Demo.DataAccess.Models.EmployeeModule;

namespace Demo.BusinessLogic.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Employee, EmployeeDto>()
                 .ForMember(dest => dest.Gender, option => option.MapFrom(Src => Src.Gender))
                 .ForMember(dest => dest.EmployeeType, option => option.MapFrom(Src => Src.EmployeeType))
                 .ForMember(dest => dest.Department, option => option.MapFrom(Src => Src.Department != null ? Src.Department.Name : null ));

                  
            CreateMap<Employee, EmployeeDetailsDto>()
                 .ForMember(dest => dest.Gender, option => option.MapFrom(Src => Src.Gender))
                 .ForMember(dest => dest.EmployeeType, option => option.MapFrom(Src => Src.EmployeeType))
                 .ForMember(dest => dest.HiringDate , option => option.MapFrom(src => DateOnly.FromDateTime(src.HiringDate)))
                 .ForMember(dest => dest.Department, option => option.MapFrom(Src => Src.Department != null ? Src.Department.Name : null));

            CreateMap<CreateEmployeeDto, Employee>()
            .ForMember(dest => dest.HiringDate, option => option.MapFrom(src => src.HiringDate.ToDateTime(TimeOnly.MinValue)));   

            CreateMap<UpdatedEmployeeDto, Employee>()
            .ForMember(dest => dest.HiringDate, option => option.MapFrom(src => src.HiringDate.ToDateTime(TimeOnly.MinValue)));




        }
    }
}
