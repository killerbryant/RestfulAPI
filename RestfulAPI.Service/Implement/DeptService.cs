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
    public class DeptService : IDeptService
    {
        /// <summary>
        /// 注入介面
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// 構造函數
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
        public DeptService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public int AddDept(DeptDto dept)
        {
            // 使用AutoMapper進行物件轉換
            var info = _mapper.Map<DeptDto, Departments>(dept);
            _unitOfWork.GetRepository<Departments>().Add(info);
            int count = _unitOfWork.SaveChanges();
            return count;
        }

        public List<DeptDto> GetAllDept()
        {
            List<Departments> users = _unitOfWork.GetRepository<Departments>().GetAll().ToList();
            List<DeptDto> datas = _mapper.Map<List<Departments>, List<DeptDto>>(users);
            return datas;
        }

        public DeptDto GetOneDept(string id)
        {
            Departments tbDept = _unitOfWork.GetRepository<Departments>().Get(id);
            DeptDto dept = _mapper.Map<Departments, DeptDto>(tbDept);
            return dept;
        }
    }
}
