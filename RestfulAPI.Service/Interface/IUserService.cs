using RestfulAPI.Model.Models.Dto;
using System.Collections.Generic;

namespace RestfulAPI.Service.Interface
{
    public interface IUserService
    {
        void AddUser(UserDto user);

        List<UserDto> GetAllUser();

        UserDto GetOneUser(string id);
    }
}
