using SimuladorVotos.BLL;
using SimuladorVotos.EF;
using SimuladorVotos.Models;
using System;
using System.CodeDom;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;

namespace SimuladorVotos.Controllers
{
    public class ConfiguracaoController : Controller
    {
        JavaScriptResult Script => new JavaScriptResult();
        ConfiguracaoBLL BLL => new ConfiguracaoBLL();
        RoboBLL BLLRobo => new RoboBLL();

        public ActionResult Index()
        {
            Session["FoiIniciado"] = BLL.FoiIniciado();
            return View();
        }

        #region Post
        [HttpPost]
        public ActionResult SaveEditConfig(FormCollection collection)
        {
            try
            {
                if (!ValidarCamposInt(collection))
                    throw new Exception("Erro ao salvar configuração. Erro durante a conversão. Verifique os campos númericos, informe valores válidos.");
                int id = BLL.SaveOrEditConfiguration(new Parametro()
                {
                    Navegadores = Convert.ToInt32(collection["cfgQtdNavegador"]),
                    SenhaPadrao = collection["cfgSenhaPadrao"].ToString(),
                    SenhaTroca = collection["cfgSenhaTroca"].ToString(),
                    URL = collection["cfgUrl"].ToString(),
                    NumeroDeChapas = collection["cfgNumChapas"].AsInt(),
                    NumeroDeVotacoes = collection["cfgNumVotacoes"].AsInt(),
                    DistribuirAutomaticamente = collection["ckDistribuicaoAutomatica"].AsBool(false),
                    VotosPorNavegador = collection["ckDitribuicaoAutomatica"].AsInt(0)
                });
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message });
            }
            return Json(new { msg = "Alterações salvas com sucesso." });
        }



        [HttpPost]
        public ActionResult SaveEditRobo(FormCollection collection, int id)
        {
            string msg = id <= 0 ? "Registro criado com sucesso." : "Registro alterado com sucesso.";
            try
            {
                if (string.IsNullOrEmpty("txtNome"))
                    throw new Exception("Campo nome não informado.");

                BLLRobo.SaveOrEdit(new Robo
                {
                    Nome = collection["txtNome"],
                    Chapa = SafeToInt(collection["nmChapaVotar"].ToString()),
                    QtdVotos = collection["nmQtdVoto"].AsInt(0),
                    QtdVotosBranco = collection["nmQtdBranco"].AsInt(0),
                    QtdVotosNulo = collection["nmQtdNulo"].AsInt(0),
                    Regional = collection["nmRegional"].AsInt(0),
                    Navegadores = collection["nmQtdNavegadores"].AsInt(0),
                    UF = collection["txtUF"].ToString(),
                    ID = id
                });
            }
            catch (Exception ex)
            {
                msg = (id > 0 ? "Erro ao editar: " : "Erro ao salvar: ") + ex.Message;
                return Json(new { msg = msg });
            }
            return Json(new { msg = msg });

        }

        [HttpPost]
        public ActionResult Iniciar(FormCollection collection)
        {
            string msg = "";
            bool ok = false;
            try
            {
                var param = BLL.BuscarParametro();
                if (param is null)
                    throw new Exception("Não foram encontrados parâmetros de configuração, impossível iniciar.");
                else if (param.SenhaPadrao != collection["cfgStartPassword"])
                    throw new Exception("Senha informada não confere.");
                else if (new VotanteBLL().Count() <= 0)
                    throw new Exception("Nenhum votante localizado na base de dados.");
                else
                {
                    param.Iniciado = true;
                    new RoboBLL().Distribuir();
                    BLL.SaveOrEditConfiguration(param);
                    msg = "O sistemas foram inicializados, nenhuma configuração adicional será possível.";
                    ok = true;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { msg = msg, ok = ok });
        }

        [HttpPost]
        public ActionResult DeleteRobo(int id)
        {
            try
            {
                BLLRobo.Delete(id);
                return Json(new { msg = "Robô excluido com sucesso." });
            }
            catch (Exception ex)
            {
                var error = ex.InnerException?.InnerException?.Message ?? "";
                    
                if (error.Contains("REFERENCE"))
                    return Json(new { msg = "Erro ao tentar excluir robô, existe um votante vinculado ao mesmo." });

                return Json(new { msg = ex.Message });
            }
        }
        #endregion

        #region GET

        [HttpGet]
        public ActionResult GetConfig()
        {
            return Json(BLL.BuscarParametro(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllRobo()
        {
            return Json(new RoboBLL().GetAll(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRobo(int id)
        {
            return Json(new RoboBLL().GetById(id), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCountVotante()
        {
            return Json(new { Qtd = new VotanteBLL().Count() }, JsonRequestBehavior.AllowGet);
        }

        public static int SafeToInt(string s, int _default = 0)
        {
            int number;
            if (int.TryParse(s, out number))
                return number;
            return _default;
        }

        #endregion

        #region PRIVATE
        private static class JSMethods
        {
            public const string ModalClear = "clearModal();";
            public const string TableLoad = "loadTable();";
        }

        private bool ValidarCamposInt(FormCollection collection)
        {
            return
           (int.TryParse(collection["cfgQtdNavegador"], out int result));
        }
        #endregion
    }
}