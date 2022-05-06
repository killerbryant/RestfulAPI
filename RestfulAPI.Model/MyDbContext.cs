using Microsoft.EntityFrameworkCore;
using RestfulAPI.Model.Models;

namespace RestfulAPI.Model
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }
        //定義資料集合：用於建立資料表
        public DbSet<User> Users { get; set; }

        public DbSet<Departments> Departments { get; set; }
    }
}

