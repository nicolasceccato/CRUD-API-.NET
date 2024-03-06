using ApiCrud.Students;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Data;

public class AppDbContext : DbContext
{
    private DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Banco.sqlite");
        base.OnConfiguring(optionsBuilder);
    }
}