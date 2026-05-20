using LiveQ.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LiveQ.Data.Seed
{
    /// <summary>
    /// Classe de extensão para injetar a inicialização da base de dados no pipeline da aplicação.
    /// Mantém o ficheiro Program.cs limpo e organizado.
    /// </summary>
    internal static class DbInitializerExtension
    {
        public static IApplicationBuilder UseItToSeedSqlServer(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            // Cria um scope de serviços temporário para aceder à Base de Dados de forma segura
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                DbInitializer.Initialize(context); // Invoca a lógica de sementeira
            }
            catch (Exception)
            {
                // Em ambiente de produção, este bloco faria log do erro
            }

            return app;
        }
    }

    /// <summary>
    /// Classe responsável por popular a base de dados com dados iniciais e estruturais.
    /// </summary>
    internal class DbInitializer
    {
        internal static void Initialize(ApplicationDbContext dbContext)
        {
            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));

            // Garante que a BD e as tabelas existem antes de inserir dados 
            dbContext.Database.EnsureCreated();

            // Variável de controlo para saber se houve alterações que precisem de ser guardadas 
            bool haAdicao = false;

            /* *****************************************************************
             * 1. CRIAÇÃO DE ROLES (PAPÉIS)
             * ***************************************************************** */
            if (!dbContext.Roles.Any())
            {
                dbContext.Roles.AddRange(
                    new IdentityRole { Id = "role_orador", Name = "Orador", NormalizedName = "ORADOR" },
                    new IdentityRole { Id = "role_participante", Name = "Participante", NormalizedName = "PARTICIPANTE" }
                );
                haAdicao = true;
            }

            /* *****************************************************************
             * 2. CRIAÇÃO DAS CREDENCIAIS DE TESTE E ASSOCIAÇÃO AOS PAPÉIS
             * ***************************************************************** */
            var users = Array.Empty<IdentityUser>();

            // Hasher para encriptar a password antes de a guardar na base de dados (Requisito de Segurança) 
            var hasher = new PasswordHasher<IdentityUser>();

            if (!dbContext.Users.Any())
            {
                // Criação do Admin (Orador) referenciado no Relatório Técnico
                var adminUser = new IdentityUser
                {
                    Id = "user_admin_01",
                    UserName = "admin@liveq.pt",
                    NormalizedUserName = "ADMIN@LIVEQ.PT",
                    Email = "admin@liveq.pt",
                    NormalizedEmail = "ADMIN@LIVEQ.PT",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin@LiveQ123!");

                // Criação do Participante Base
                var participanteUser = new IdentityUser
                {
                    Id = "user_participante_01",
                    UserName = "user@liveq.pt",
                    NormalizedUserName = "USER@LIVEQ.PT",
                    Email = "user@liveq.pt",
                    NormalizedEmail = "USER@LIVEQ.PT",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                participanteUser.PasswordHash = hasher.HashPassword(participanteUser, "User@LiveQ123!");

                users = new[] { adminUser, participanteUser };
                dbContext.Users.AddRange(users);

                // Associar os utilizadores aos respectivos Roles 
                dbContext.UserRoles.AddRange(
                    new IdentityUserRole<string> { UserId = adminUser.Id, RoleId = "role_orador" },
                    new IdentityUserRole<string> { UserId = participanteUser.Id, RoleId = "role_participante" }
                );

                haAdicao = true;
            }

            /* *****************************************************************
             * 3. DADOS FALSOS (MOCK DATA) PARA TESTES
             * ***************************************************************** */
            // Cria um Evento automático para termos conteúdo visível ao abrir o site
            if (!dbContext.Events.Any() && users.Length > 0)
            {
                var mockEvent = new Event
                {
                    Title = "Aula de Desenvolvimento Web",
                    Description = "Evento de demonstração gerado automaticamente pelo Seed da BD.",
                    CreatedAt = DateTime.UtcNow,
                    CreatorId = users[0].Id // O admin_01 é o dono do evento
                };
                dbContext.Events.Add(mockEvent);
                haAdicao = true;
            }

            /* *****************************************************************
             * GUARDAR ALTERAÇÕES NA BASE DE DADOS
             * ***************************************************************** */
            try
            {
                if (haAdicao)
                {
                    dbContext.SaveChanges(); // Torna os dados persistentes no SQL Server
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}