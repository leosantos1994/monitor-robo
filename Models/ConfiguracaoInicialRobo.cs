using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimuladorVotos.Models
{
    public class ConfiguracaoInicialRobo
    {
        public string MensagemRetorno { get; set; }
        public string Status { get; set; }
        public string URL { get; set; }
        public int NumeroDeChapas { get; set; }
        public int NumeroDeVotacoes { get; set; }
        public string SenhaPadrao { get; set; }
        public string SenhaTroca { get; set; }
        public List<VotanteExportar> Votantes { get; set; }
        public Dictionary<string, CampoRespostaPergunta> PerguntaResposta { get; set; }

        public Robo Robo { get; set; }
    }
}