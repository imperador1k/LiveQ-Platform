using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LiveQ.Data.Models;

namespace LiveQ.Data
{
    /// <summary>
    /// Classe de contexto da base de dados. Faz a ponte entre os modelos (C#) e a base de dados SQL (Entity Framework Core).
    /// </summary>
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {

        // Mapeamento das tabelas de negócio
        public DbSet<Event> Events { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Upvote> Upvotes { get; set; }


        /// <summary>
        /// Configuração fluente (Fluent API) para definições avançadas da base de dados 
        /// que não podem ser alcançadas apenas com Data Annotations.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuração da Chave Primária Composta para a tabela Upvote
            builder.Entity<Upvote>()
                .HasKey(u => new { u.QuestionId, u.UserId });

            // FIX: Emendamos o erro "Multiple Cascade Paths" do SQL Server.
            // Desligamos a eliminação em cascata entre User e o Upvote.
            // Se um User for apagado, a BD não tenta apagar os seus votos de forma automática por este caminho.
            builder.Entity<Upvote>()
                .HasOne(u => u.User)
                .WithMany() // Um User tem muitos Upvotes (mas não definimos a lista reversa no IdentityUser)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

