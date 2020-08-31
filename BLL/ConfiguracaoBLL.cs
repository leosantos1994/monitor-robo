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
        public int SaveOrEditConfiguration(Configuracao Model)
        {
            using (var ctx = new EFContext())
            {
                var cfg = ctx.Configuracao.FirstOrDefault()?? new Configuracao();
                cfg.Navegadores = Model.Navegadores;
                cfg.SenhaPadrao = Model.SenhaPadrao;
                cfg.VotosPorNavegador = Model.VotosPorNavegador;
                if (cfg.ID > 0)
                {
                    ctx.Configuracao.Attach(cfg);
                    ctx.Entry(cfg).State = EntityState.Modified;
                    
                }
                else
                {
                    cfg.CriadoEm = DateTime.Now;
                    ctx.Configuracao.Add(cfg);
                }

                ctx.SaveChanges();
                return cfg.ID;
            }
        }

        public int SaveOrEditServer(Servidores Model)
        {
            using (var ctx = new EFContext())
            {
                if (Model.ID > 0)
                {
                    ctx.Servidores.Attach(Model);
                    ctx.Entry(Model).State = EntityState.Modified;
                }
                else
                {
                    Model.CriadoEm = DateTime.Now;
                    ctx.Servidores.Add(Model);
                }

                ctx.SaveChanges();
                return Model.ID;
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new EFContext())
            {
                var employer = new Servidores { ID = id };
                ctx.Entry(employer).State = EntityState.Deleted;
                ctx.SaveChanges();
            }
        }
    }
}