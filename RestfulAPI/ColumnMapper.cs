using Dapper;
using RestfulAPI.Model;
using RestfulAPI.Model.Models;

namespace RestfulAPI
{
    public class ColumnMapper
    {
        public static void SetMapper()
        {
            //資料庫欄位名和c#屬性名不一致，手動添加映射關係
            SqlMapper.SetTypeMap(typeof(User), new ColumnAttributeTypeMapper<User>());

            //每個需要用到[ColumnName(Name="")]特性的model，都要在這裡添加映射
        }

    }
}
