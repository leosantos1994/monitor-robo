using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimuladorVotos.Models
{
    public class Votante
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public int Regional { get; set; }
        public TipoVoto TipoVoto { get; set; }
        public StatusVotante Status { get; set; }
        public string NomeMae { get; set; }
        public string DataNascimento { get; set; }
        public DateTime? CriadoEm { get; set; }
        public DateTime? AtualizadoEm { get; set; }
        public int? RoboID { get; set; }
        [JsonIgnore]
        public virtual Robo? Robo { get; set; }
    }

    public enum StatusVotante
    {
        Null,
        TrocouSenha,
        Votou
    }

    public enum TipoVoto
    {
        Branco,
        Nulo,
        Chapa
    }

    public enum CampoRespostaPergunta
    {
        Null,
        DataNascimento,
        CPF,
        NomeMae
    }
}