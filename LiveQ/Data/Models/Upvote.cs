using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveQ.Data.Models
{
    /// <summary>
    /// Entidade associativa que resolve a relação Muitos-para-Muitos (M-N) entre Utilizadores e Perguntas.
    /// Regista a ação de voto, garantindo a integridade de "um voto por utilizador por pergunta".
    /// </summary>
    public class Upvote
    {
        /// <summary>
        /// Chave Estrangeira e parte da Chave Primária Composta, associada à Pergunta.
        /// </summary>
        [ForeignKey(nameof(Question))]
        public int QuestionId { get; set; }

        /// <summary>
        /// Propriedade de navegação para a Pergunta alvo do voto.
        /// </summary>
        [ValidateNever]
        public Question Question { get; set; } = null!;

        /// <summary>
        /// Chave Estrangeira e parte da Chave Primária Composta, associada ao Utilizador que votou.
        /// </summary>
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Propriedade de navegação para o Utilizador eleitor.
        /// </summary>
        [ValidateNever]
        public IdentityUser User { get; set; } = null!;
    }
}