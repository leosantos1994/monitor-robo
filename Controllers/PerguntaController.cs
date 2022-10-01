using SimuladorVotos.BLL;
using SimuladorVotos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace SimuladorVotos.Controllers
{
    public class PerguntaController : Controller
    {
        // GET: Pergunta
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            try
            {
                if (string.IsNullOrEmpty(collection["txtPergunta"]))
                    throw new Exception("Campo Pergunta é obrigatório");
                if (string.IsNullOrEmpty(collection["slPergunta"]))
                    throw new Exception("Campo Resposta é obrigatório");
                if (!string.IsNullOrEmpty(collection["id"]))
                {
                    new PerguntaBLL().Edit(new Models.PerguntaResposta()
                    {
                        CampoRespostaPergunta = (Models.CampoRespostaPergunta)collection["slPergunta"].AsInt(0),
                        Pergunta = collection["txtPergunta"].ToString(),
                        ID = collection["id"].AsInt()
                    });
                    return Content("<script language='javascript' type='text/javascript'>alert('Pergunta alterada com sucesso.');" +
                           $"window.location.href = '{this.Request.Url.OriginalString}'</script>");
                }
                else
                {
                    new PerguntaBLL().Add(new Models.PerguntaResposta()
                    {
                        CampoRespostaPergunta = (Models.CampoRespostaPergunta)collection["slPergunta"].AsInt(0),
                        Pergunta = collection["txtPergunta"].ToString(),
                    });
                    return Content("<script language='javascript' type='text/javascript'>alert('Pergunta adicionada com sucesso.');" +
                        $"window.location.href = '{this.Request.Url.OriginalString}'</script>");
                }

            }
            catch
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Erro ao adicionar pergunta.');" +
                $"window.location.href = '{this.Request.Url.OriginalString}'</script>");
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                new PerguntaBLL().Delete(id);
                return Json(new { msg = "Pergunta excluída com sucesso." });
            }
            catch
            {
                return Json(new { msg = "Erro ao excluir pergunta." });
            }
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            return Json(new PerguntaBLL().GetAll().ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            try
            {
                return Json(new PerguntaBLL().GetById(id), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { msg = "Erro ao buscar pergunta.", data = new PerguntaResposta() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}