using SimuladorVotos.EF;
using SimuladorVotos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SimuladorVotos.BLL
{
    public class ConfiguracaoBLL
    {
        public int SaveOrEditConfiguration(Parametro Model)
        {
            //Apenas existirá um parametro ativo durante o ciclo de vida do gestor
            //está consulta não resulta em performance, pois o framework reserva os dados em memoria
            //essa consulta não pode estar no mesmo contexto da segunda
            using (var ctx = new GerenciadorContext())
            {
                Model.ID = ctx.Parametro?.FirstOrDefault()?.ID ?? 0;
            }

            using (var ctx = new GerenciadorContext())
            {
                Model.UltimaAlteracao = DateTime.Now;

                if (Model.ID > 0)
                {
                    ctx.Parametro.Attach(Model);
                    ctx.Entry(Model).State = EntityState.Modified;
                }
                else
                    ctx.Parametro.Add(Model);

                ctx.SaveChanges();

                return ctx.Parametro.FirstOrDefault().ID;
            }
        }

        public void AddRemoveVotanteOnline(bool add)
        {
            Parametro param = new Parametro();
            using (var ctx = new GerenciadorContext())
            {
                param = ctx.Parametro?.FirstOrDefault();
            }

            using (var ctx = new GerenciadorContext())
            {
                param.VotantesDoRoboOnline = add ? param.VotantesDoRoboOnline + 1 : param.VotantesDoRoboOnline - 1;
                ctx.Parametro.Attach(param);
                ctx.Entry(param).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public bool FoiIniciado()
        {
            using (var ctx = new GerenciadorContext())
            {
                var result = ctx.Parametro?.FirstOrDefault();
                return result is null || !result.Iniciado ? false : true;
            }
        }

        public Parametro BuscarParametro()
        {
            using (var ctx = new GerenciadorContext())
            {
                return ctx.Parametro.FirstOrDefault();
            }
        }
    }
}