using AutoMapper;
using RestfulAPI.Model.Models;
using RestfulAPI.Model.Models.Dto;

namespace RestfulAPI
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();

            CreateMap<DeptDto, Departments>();
            CreateMap<Departments, DeptDto>();
        }
    }
}
