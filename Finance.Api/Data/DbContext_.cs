
using Finance.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Finance.Api.Data
{
    public class DbContext_ : DbContext
    {
        public DbContext_(DbContextOptions<DbContext_> options) : base(options)
        {
        }

        // Criação das tabelas no banco de dados
        public DbSet<ContaMovimento> contaMovimentoTbl {  get; set; }
        public DbSet<Transacao> transacaotbl {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Registrar número de casas decimais
            modelBuilder.Entity<ContaMovimento>()
                .Property(conta => conta.Saldo)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transacao>()
                .Property(conta => conta.Valor)
                .HasPrecision(18, 2);
        }

    }
}
