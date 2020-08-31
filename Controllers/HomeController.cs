using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using SimuladorVotos.Models;

namespace SimuladorVotos.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            //using (var ctx = new EFContext())
            //{
            //    //Exibe o SQL gerado na janela Debug      
            //    ctx.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            //    var teste = (from a in ctx.Servidores
            //                 where !string.IsNullOrEmpty(a.URL)
            //                 select a).FirstOrDefault<Servidores>();
            //}

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}