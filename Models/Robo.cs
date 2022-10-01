using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SimuladorVotos.Models
{
    public class Robo
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Nome { get; set; }
        public int QtdVotosBranco { get; set; }
        public int QtdVotosNulo{ get; set; }
        public int QtdVotos { get; set; }
        public int Chapa { get; set; }
        public int Navegadores { get; set; }
        public int Regional { get; set; }
        public String UF { get; set; }
        public System.DateTime? CriadoEm { get; set; }
        public System.DateTime? AtualizadoEm { get; set; }
        public Status Status { get; set; } 
    }

    public enum Status
    {
        NaoIniciado,
        Ativado,
        Concluido
    }
}