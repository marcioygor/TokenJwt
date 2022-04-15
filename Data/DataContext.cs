using Microsoft.EntityFrameworkCore;
using Api_Carro.Models;

namespace Api_Carro.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Carro> Carros { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

    }
}