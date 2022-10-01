using SimuladorVotos.EF;
using SimuladorVotos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SimuladorVotos.Helper;
using Microsoft.Ajax.Utilities;
using System.Data.Entity.Migrations;

namespace SimuladorVotos.BLL
{
    public class RoboBLL
    {
        public int SaveOrEdit(Robo Model)
        {
            using (var ctx = new GerenciadorContext())
            {
                if (Model.ID > 0)
                {
                    Model.AtualizadoEm = DateTime.Now;
                }
                else
                {
                    Model.CriadoEm = DateTime.Now;
                    Model.AtualizadoEm = DateTime.Now;
                    Model.Status = Status.NaoIniciado;
                }
                ctx.Set<Robo>().AddOrUpdate(Model);
                ctx.SaveChanges();
                return Model.ID;
            }
        }

        public List<Robo> GetAll()
        {
            using (var ctx = new GerenciadorContext())
            {
                return (from a in ctx.Robo select a).ToList();
            }
        }

        public Robo GetById(int Id)
        {
            using (var ctx = new GerenciadorContext())
            {
                return (ctx.Robo.Where(x => x.ID == Id).FirstOrDefault());
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new GerenciadorContext())
            {
                var robo = new Robo { ID = id };
                ctx.Entry(robo).State = EntityState.Deleted;
                ctx.SaveChanges();
            }
        }


        public void Distribuir()
        {
            var config = new ConfiguracaoBLL().BuscarParametro();

            if (config.DistribuirAutomaticamente)
                DistribuirVotantesAutomaticamente();
            else DistribuirManualmente();
        }

        private void DistribuirManualmente()
        {
            try
            {
                var robos = GetAll();
                var bllVotantes = new VotanteBLL();
                var votantes = bllVotantes.GetAll();
                foreach (var robo in robos)
                {
                    int take = robo.QtdVotosBranco + robo.QtdVotosNulo + robo.QtdVotos;
                    var votantesDoRobo =
                        robo.Regional > 0 ? votantes.Where(x => x.Regional == robo.Regional).ToList().GetRange(0, take).ToList()
                        : votantes.GetRange(0, take).ToList();
                    votantesDoRobo.ForEach(x => votantes.Remove(x));

                    //pega a quantidade de brancos e remove-os da lista
                    var listaBrancos = votantesDoRobo.Take(robo.QtdVotosBranco).ToList();
                    listaBrancos.ForEach(x => votantesDoRobo.Remove(x));
                    listaBrancos.ForEach(x => x.TipoVoto = TipoVoto.Branco);

                    //pega a quantidade de nulos e remove-os da lista
                    var listaNulos = votantesDoRobo.Take(robo.QtdVotosNulo).ToList();
                    listaNulos.ForEach(x => votantesDoRobo.Remove(x));
                    listaNulos.ForEach(x => x.TipoVoto = TipoVoto.Nulo);

                    var listaVotos = votantesDoRobo.Take(robo.QtdVotos).ToList();
                    listaVotos.ForEach(x => x.TipoVoto = TipoVoto.Chapa);

                    var listaGeral = new List<Votante>();
                    listaGeral.AddRange(listaBrancos);
                    listaGeral.AddRange(listaNulos);
                    listaGeral.AddRange(listaVotos);

                    foreach (var item in listaGeral)
                    {
                        item.RoboID = robo.ID;
                        bllVotantes.Edit(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro na distribuição manual, " +
                    "os parâmetros do robô devem respeitar as configurações dos votantes importados, detalhe: " + ex.Message);
            }
        }

        private void DistribuirVotantesAutomaticamente()
        {
            try
            {
                var bllVotante = new VotanteBLL();
                var config = new ConfiguracaoBLL().BuscarParametro();

                var robos = GetAll().ToList();
                var votantes = bllVotante.GetAll();
                int valorDivisaoFinal = (int)Math.Ceiling((decimal)(votantes.Count / robos.Count));
                var votantesDivididosPelaQuantidadeDeRobos = votantes.Split(valorDivisaoFinal).ToList();
                var listaVotantesFinal = new List<Votante>();


                var listRestos = new List<Votante>();
                for (int i = 0; i < robos.Count; i++)
                {
                    var robo = robos[i];
                    var votantesDoRobo = new List<Votante>();
                    if (i == robos.Count - 1)
                    {
                        votantesDivididosPelaQuantidadeDeRobos.ForEach(x => x.ForEach(y => listRestos.Add(y)));
                        votantesDoRobo = listRestos;
                    }
                    else
                    {
                        votantesDoRobo = votantesDivididosPelaQuantidadeDeRobos.FirstOrDefault().ToList();
                        votantesDivididosPelaQuantidadeDeRobos.RemoveRange(0, 1);
                    }

                    //atribui o robô
                    int ultimoVoto = 0;
                    foreach (var votante in votantesDoRobo)
                    {
                        votante.RoboID = robo.ID;
                        ultimoVoto = RandomControl(ultimoVoto);
                        votante.TipoVoto = (TipoVoto)ultimoVoto;
                        votante.AtualizadoEm = DateTime.Now;
                    }
                    //Adiciona na lista geral
                    listaVotantesFinal.AddRange(votantesDoRobo);

                    if (robo.Navegadores <= 0)
                        robo.Navegadores = config.Navegadores;

                    //Votantes em alguma chapa
                    robo.QtdVotos = votantesDoRobo.Where(x => x.TipoVoto == TipoVoto.Chapa).Count();
                    //Votantes em branco
                    robo.QtdVotosBranco = votantesDoRobo.Where(x => x.TipoVoto == TipoVoto.Branco).Count();
                    //Votantes Nulo
                    robo.QtdVotosNulo = votantesDoRobo.Where(x => x.TipoVoto == TipoVoto.Nulo).Count();
                }

                robos.ForEach(x => SaveOrEdit(x));
                bllVotante.Edit(listaVotantesFinal.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception("Erro na distribuição automática, detalhe: " + ex.Message);
            }
        }

        private int RandomControl(int lastRandom)
        {
            int random = 0;
            do
            {
                random = new Random().Next(0, 3);
            }
            while (lastRandom == random);
            return random;
        }
    }
}
