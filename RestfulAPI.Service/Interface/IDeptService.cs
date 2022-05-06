using RestfulAPI.Model.Models.Dto;
using System.Collections.Generic;

namespace RestfulAPI.Service.Interface
{
    public interface IDeptService
    {
        int AddDept(DeptDto dept);

        List<DeptDto> GetAllDept();

        DeptDto GetOneDept(string id);
    }
}
