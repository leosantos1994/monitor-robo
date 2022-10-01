using SimuladorVotos.BLL;
using SimuladorVotos.Helper;
using SimuladorVotos.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;

namespace SimuladorVotos.Controllers
{
    public class StartController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetIniciado()
        {
            var iniciado = new ConfiguracaoBLL().FoiIniciado();
            return Ok(iniciado);
        }

        [HttpGet]
        public IHttpActionResult GetConfiguracao(int roboId)
        {
            var modelRetorno = new ConfiguracaoInicialRobo();
            try
            {
                var cfg = new ConfiguracaoBLL().BuscarParametro();

                var votantes = new VotanteBLL().GetByRobo(roboId);

                modelRetorno.SenhaPadrao = cfg.SenhaPadrao;
                modelRetorno.SenhaTroca = cfg.SenhaTroca;
                if (votantes is null || votantes.Count <= 0)
                    throw new Exception("Nenhum votante a processar.");

                modelRetorno.Votantes = MapToVotanteExportar(votantes).ToList();
                modelRetorno.Robo = votantes.FirstOrDefault().Robo;
                modelRetorno.Status = "OK";
                modelRetorno.URL = cfg.URL;
                modelRetorno.NumeroDeVotacoes = cfg.NumeroDeVotacoes;
                modelRetorno.NumeroDeChapas = cfg.NumeroDeChapas;
                modelRetorno.Robo.Status = Status.Ativado;
                modelRetorno.PerguntaResposta = BuscarPerguntas();
                new RoboBLL().SaveOrEdit(modelRetorno.Robo);

            }
            catch (Exception ex)
            {
                modelRetorno.Status = "Error";
                modelRetorno.MensagemRetorno = ex.Message;
            }
            return Ok(modelRetorno);
        }

        [HttpPost]
        public IHttpActionResult PostStatusVotante(int votanteId, int status)
        {
            new VotanteBLL().EditStatus(votanteId, (StatusVotante)status);
            return Ok("");
        }

        [HttpPost]
        [Route("api/start/PostAddVotanteOnline")]
        public IHttpActionResult PostAddVotanteOnline()
        {
            new ConfiguracaoBLL().AddRemoveVotanteOnline(true);
            return Ok("");
        }

        [HttpPost]
        [Route("api/start/PostRemoveVotanteOnline")]
        public IHttpActionResult PostRemoveVotanteOnline()
        {
            new ConfiguracaoBLL().AddRemoveVotanteOnline(false);
            return Ok("");
        }

        private Dictionary<string, CampoRespostaPergunta> BuscarPerguntas()
        {
            var perguntas = new PerguntaBLL().GetAll();
            var dic = new Dictionary<string, CampoRespostaPergunta>();
            foreach (var item in perguntas)
            {
                dic.Add(item.Pergunta, item.CampoRespostaPergunta);
            }
            return dic;
        }

        private IEnumerable<VotanteExportar> MapToVotanteExportar(List<Votante> votantes)
        {
            foreach (var item in votantes)
                yield return item.Map<VotanteExportar>();
        }
    }
}
