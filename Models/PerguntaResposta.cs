using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimuladorVotos.Models
{
    public class PerguntaResposta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Pergunta { get; set; }
        public CampoRespostaPergunta CampoRespostaPergunta { get; set; }
    }
}