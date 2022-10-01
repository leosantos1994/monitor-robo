using Microsoft.Ajax.Utilities;
using SimuladorVotos.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SimuladorVotos.EF
{
    public class GerenciadorContext : DbContext
    {
        string conn = ConfigurationManager.AppSettings.Get("DBName");
        public GerenciadorContext() : base()
        {
            this.Database.Connection.ConnectionString = conn;
        }

        public DbSet<Parametro> Parametro { get; set; }
        public DbSet<Robo> Robo { get; set; }
        public DbSet<Votante> Votante { get; set; }
        public DbSet<PerguntaResposta> PerguntaResposta { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);

        }
    }

}