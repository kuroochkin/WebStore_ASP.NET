using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;

namespace WebStore.DAL.Context
{
    public class WebStoreDB : IdentityDbContext<User,Role,string> // string - это ключ
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public WebStoreDB(DbContextOptions<WebStoreDB> options) : base(options)
        {

        }
    }
}
