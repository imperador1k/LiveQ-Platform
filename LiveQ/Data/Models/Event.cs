using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveQ.Data.Models
{
    /// <summary>
    /// Entidade que representa uma sala de perguntas e respostas (Evento).
    /// Criada pelo papel de Orador para centralizar a interação com a audiência.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Chave Primária (PK) da tabela. Identificador único do evento.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Título descritivo do evento.
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        [StringLength(200)]
        [Display(Name = "Título da Sala")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Descrição detalhada ou contexto adicional do evento.
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "Descrição")]
        public string? Description { get; set; }

        /// <summary>
        /// Registo temporal da criação do evento (formato UTC para consistência global).
        /// </summary>
        [Display(Name = "Data de Criação")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;




        /* ****************************************
         * Construção dos Relacionamentos
         * *************************************** */

        // Relacionamento 1-N (Utilizador cria múltiplos Eventos)

        /// <summary>
        /// Chave Estrangeira (FK) que liga o evento ao utilizador (Orador) que o criou.
        /// </summary>
        [Required]
        [ForeignKey(nameof(Creator))]
        public string CreatorId { get; set; } = string.Empty;

        /// <summary>
        /// Propriedade de navegação para o utilizador criador.
        /// A anotação ValidateNever impede que o ModelState exija este objeto na submissão de formulários.
        /// </summary>
        [ValidateNever]
        public IdentityUser Creator { get; set; } = null!;

        // Relacionamento 1-N (Evento contém múltiplas Perguntas)

        /// <summary>
        /// Coleção de perguntas submetidas no contexto deste evento.
        /// </summary>
        [ValidateNever]
        public ICollection<Question> QuestionsList { get; set; } = new List<Question>();
    }
}