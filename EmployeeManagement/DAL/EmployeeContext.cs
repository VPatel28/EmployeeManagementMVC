using EmployeeManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.DAL
{
    public class EmployeeContext : IdentityDbContext<ApplicationUser>
    {
        //public EmployeeContext() : base("EmployeeContext")
        //{

        //}

        public EmployeeContext(DbContextOptions<EmployeeContext> options)
          : base(options)
        {
        }

        public DbSet<Department> departments { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<Registration_Details> registration_Details { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EmployeeManagement;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

    }
}
