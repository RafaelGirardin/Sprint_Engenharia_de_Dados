using Microsoft.EntityFrameworkCore;

namespace Scraping_TJSP
{
    public class DatabaseConfiguracao : DbContext
    {
        public virtual DbSet<Decisao> Decisao { get; set; }

        public virtual DbSet<Relator> Relator { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=RafaelMGPF\\SQLEXPRESS;Initial Catalog=BD_Sprint_Engenharia_de_Dados;Integrated Security=True;TrustServerCertificate=True");
        }

    }
}