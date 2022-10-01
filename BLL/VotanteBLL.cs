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
    public class VotanteBLL
    {
        public int Add(Votante Model)
        {
            using (var ctx = new GerenciadorContext())
            {
                ctx.Set<Votante>().AddOrUpdate(Model);
                ctx.SaveChanges();
                return Model.ID;
            }
        }

        public void Edit(Votante[] Models)
        {
            using (var ctx = new GerenciadorContext())
            {
                ctx.Set<Votante>().AddOrUpdate(Models);
                ctx.SaveChanges();
            }
        }

        public void Edit(Votante Model)
        {
            using (var ctx = new GerenciadorContext())
            {
                ctx.Set<Votante>().AddOrUpdate(Model);
                ctx.SaveChanges();
            }
        }

        public void EditStatus(int votante, StatusVotante status)
        {
            using (var ctx = new GerenciadorContext())
            {
                var model = ctx.Votante.Where(x => x.ID == votante).FirstOrDefault();
                model.Status = status;
                ctx.Set<Votante>().AddOrUpdate(model);
                ctx.SaveChanges();
            }
        }

        public List<Votante> GetAll()
        {
            using (var ctx = new GerenciadorContext())
            {
                return (from a in ctx.Votante select a).ToList();
            }
        }

        public List<Votante> GetByRobo(int robo)
        {
            using (var ctx = new GerenciadorContext())
            {
                var items = (from a in ctx.Votante.Include(x=> x.Robo).Where(x => x.RoboID == robo && x.Status != StatusVotante.Votou) select a).ToList();
                return items.ToList();
            }
        }

        public int Count()
        {
            using (var ctx = new GerenciadorContext())
            {
                return ctx.Votante.Count();
            }
        }
    }
}