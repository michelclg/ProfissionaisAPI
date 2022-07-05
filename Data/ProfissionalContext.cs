using Microsoft.EntityFrameworkCore;
using ProfissionaisAPI.Model;

namespace ProfissionaisAPI.Data
{
    public class ProfissionalContext : DbContext
    {
        public DbSet<Profissional> Profissionais { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ProfissionaisAPI");
        }
    }
}
