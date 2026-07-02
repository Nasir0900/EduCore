using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EduCore.Models;

namespace EduCore.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Department> Departments { get; set; }

        public DbSet<Faculty> Faculties { get; set; }
    }

}
