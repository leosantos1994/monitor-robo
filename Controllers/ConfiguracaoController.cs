using SimuladorVotos.BLL;
using SimuladorVotos.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SimuladorVotos.Controllers
{
    public class ConfiguracaoController : Controller
    {
        JavaScriptResult Script => new JavaScriptResult();
        ConfiguracaoBLL BLL = new ConfiguracaoBLL();
        public ActionResult Index()
        {
            return View();
        }

        #region Post
        [HttpPost]
        public JavaScriptResult SaveEditConfig(FormCollection collection)
        {
            int id = BLL.SaveOrEditConfiguration(new Configuracao()
            {
                Navegadores = Convert.ToInt32(collection["cfgQtdNavegador"]),
                SenhaPadrao = collection["cfgQtdVotosNavegador"].ToString(),
                VotosPorNavegador = Convert.ToInt32(collection["cfgSenhaPadrao"])
            });

            Script.Script = id > 0 ? "alert('Registro criado com sucesso')" : "alert('Registro alterado com sucesso')";
            return Script;
        }

        [HttpPost]
        public JavaScriptResult SaveEditServer(FormCollection collection, int id)
        {
            BLL.SaveOrEditServer(new Servidores
            {
                Nome = collection["listNome"],
                URL = collection["listUrl"],
                ID = id
            });
            Script.Script = id > 0 ? "alert('Registro criado com sucesso')" : "alert('Registro alterado com sucesso')";
            return Script;
        }

        [HttpPost]
        public JavaScriptResult Delete(int id)
        {
            BLL.Delete(id);
            Script.Script = JSMethods.TableLoad;
            return Script;
        }
        #endregion

        #region GET

        [HttpGet]
        public JsonResult GetConfig()
        {
            using (var ctx = new EFContext())
            {
                var result = ctx.Configuracao.FirstOrDefault();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetAllServer()
        {
            using (var ctx = new EFContext())
            {
                var list = (from a in ctx.Servidores select a).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetServer(int id)
        {
            using (var ctx = new EFContext())
            {
                var result = ctx.Servidores.FirstOrDefault(x => x.ID == id);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        private static class JSMethods
        {
            public const string ModalClear = "clearModal();";
            public const string TableLoad = "loadTable();";
        }
    }
}