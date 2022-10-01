using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimuladorVotos.Models
{
    public class Parametro
    {
        public int ID { get; set; }
        public System.DateTime UltimaAlteracao { get; set; }
        public int Navegadores { get; set; }
        public int VotosPorNavegador { get; set; }
        public int NumeroDeChapas { get; set; }
        public int NumeroDeVotacoes { get; set; }
        public int VotantesDoRoboOnline { get; set; }
        public string SenhaPadrao { get; set; }
        public string SenhaTroca { get; set; }
        public string URL { get; set; }
        public bool Iniciado { get; set; }
        public bool DistribuirAutomaticamente { get; set; }
    }
}