using SimuladorVotos.EF;
using SimuladorVotos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace SimuladorVotos.BLL
{
    public class PerguntaBLL
    {
        public int Add(PerguntaResposta Model)
        {
            using (var ctx = new GerenciadorContext())
            {
                ctx.Set<PerguntaResposta>().AddOrUpdate(Model);
                ctx.SaveChanges();
                return Model.ID;
            }
        }

        public void Edit(PerguntaResposta Model)
        {
            using (var ctx = new GerenciadorContext())
            {
                ctx.Set<PerguntaResposta>().AddOrUpdate(Model);
                ctx.SaveChanges();
            }
        }

        public List<PerguntaResposta> GetAll()
        {
            using (var ctx = new GerenciadorContext())
            {
                return (from a in ctx.PerguntaResposta select a).ToList();
            }
        }

        public PerguntaResposta GetById(int id)
        {
            using (var ctx = new GerenciadorContext())
            {
                return ctx.PerguntaResposta.Where(x => x.ID == id).FirstOrDefault();
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new GerenciadorContext())
            {
                var robo = new PerguntaResposta { ID = id };
                ctx.Entry(robo).State = EntityState.Deleted;
                ctx.SaveChanges();
            }
        }
    }
}