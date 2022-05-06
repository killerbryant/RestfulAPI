using AutoMapper;
using RestfulAPI.Model.Models;
using RestfulAPI.Model.Models.Dto;
using RestfulAPI.Repository.Repository;
using RestfulAPI.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestfulAPI.Service.Implement
{
    public class UserService : IUserService
    {
        /// <summary>
        /// 注入介面
        /// </summary>
        private readonly IDapperUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// 構造函數
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
        public UserService(IDapperUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void AddUser(UserDto user)
        {
            // 使用AutoMapper進行物件轉換
            var info = _mapper.Map<UserDto, User>(user);
            _unitOfWork.GetRepository<User>().Add(info);
            _unitOfWork.Commit();
        }

        public List<UserDto> GetAllUser()
        {
            List<User> users = _unitOfWork.GetRepository<User>().GetAll().ToList();
            List<UserDto> datas = _mapper.Map<List<User>, List<UserDto>>(users);
            return datas;
        }

        public UserDto GetOneUser(string id)
        {
            User tbUser = _unitOfWork.GetRepository<User>().Get(id);
            UserDto user = _mapper.Map<User, UserDto>(tbUser);
            return user;
        }
    }
}
