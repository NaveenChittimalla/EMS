using EMS.WebApi.EfCore.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApi.EfCore.Data;
public class EmsDbContext : DbContext
{
    public EmsDbContext(DbContextOptions<EmsDbContext> options)
        : base(options)
    {

    }

    public DbSet<Employee> Employee { get; set; }
}
