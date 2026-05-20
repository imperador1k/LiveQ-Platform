using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LiveQ.Data.Models
{
    /// <summary>
    /// Entidade que representa uma pergunta submetida por um participante.
    /// Pode ser associada a um utilizador autenticado ou submetida de forma anónima.
    /// </summary>
    public class Question
    {
        /// <summary>
        /// Chave Primária (PK) da tabela.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// O conteúdo textual da pergunta formulada pelo participante.
        /// </summary>
        [Required(ErrorMessage = "A {0} não pode estar vazia.")]
        [StringLength(500)]
        [Display(Name = "Pergunta")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Registo temporal da submissão da pergunta.
        /// </summary>
        [Display(Name = "Data de Submissão")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicador de estado. Se verdadeiro, a pergunta foi respondida pelo Orador
        /// e deve ser ocultada do topo da lista principal.
        /// </summary>
        [Display(Name = "Respondida")]
        public bool IsAnswered { get; set; } = false;




        /* ****************************************
         * Construção dos Relacionamentos
         * *************************************** */

        // Relacionamento 1-N (Evento contém a Pergunta)

        /// <summary>
        /// Chave Estrangeira (FK) que liga a pergunta ao evento onde foi submetida.
        /// </summary>
        [Required]
        [ForeignKey(nameof(Event))]
        public int EventId { get; set; }

        /// <summary>
        /// Propriedade de navegação para o Evento de origem.
        /// </summary>
        [ValidateNever]
        public Event Event { get; set; } = null!;

        // Relacionamento 1-N (Utilizador faz a Pergunta)

        /// <summary>
        /// Chave Estrangeira (FK) para o autor. 
        /// É anulável (nullable - string?) para suportar a regra de negócio de submissões anónimas.
        /// </summary>
        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }

        /// <summary>
        /// Propriedade de navegação para o utilizador autor da pergunta (se aplicável).
        /// </summary>
        [ValidateNever]
        public IdentityUser? User { get; set; }

        // Relacionamento 1-N para suportar a tabela de junção (Votos)

        /// <summary>
        /// Lista de votos associados a esta pergunta.
        /// </summary>
        [ValidateNever]
        public ICollection<Upvote> UpvotesList { get; set; } = new List<Upvote>();
    }
}