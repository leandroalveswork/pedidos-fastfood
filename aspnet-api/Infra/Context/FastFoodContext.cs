using AspnetApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AspnetApi.Infra.Context {
    public class FastFoodContext : DbContext {
        public FastFoodContext (DbContextOptions<FastFoodContext> options) : base (options) { }
        public DbSet<Pedido> Pedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Pedido>().ToTable("Pedidos");
            builder.Entity<Pedido>().HasKey(x => x.Id);
            builder.Entity<Pedido>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Pedido>().Property(x => x.SolicitanteId).IsRequired();
            builder.Entity<Pedido>().Property(x => x.Posicao).IsRequired();
            builder.Entity<Pedido>().Property(x => x.Lanche).IsRequired();
            builder.Entity<Pedido>().Property(x => x.Bebida).IsRequired();
        }
    }
}