using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VendinhaGR.Models;

namespace VendinhaGR.Data
{
    public class AppDbContext : DbContext
    {
        // construtor padrao pro banco funcionar
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Divida> Dividas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // garantindo que ninguem cadastre cpf repetido direto no banco
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.CPF)
                .IsUnique();

            // amarrando as dividas no cliente. se apagar o cliente, as dividas somem junto pra nao ficar sujeira
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Dividas)
                .WithOne(d => d.Cliente)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}