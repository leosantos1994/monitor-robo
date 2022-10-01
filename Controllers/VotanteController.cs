using SimuladorVotos.BLL;
using SimuladorVotos.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimuladorVotos.Controllers
{
    public class VotanteController : Controller
    {
        VotanteBLL BLL => new VotanteBLL();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            try
            {
                if (new ConfiguracaoBLL().FoiIniciado())
                    throw new Exception("Impossível incluir votantes após a incialização.");

                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        var votantes = new List<Votante>();
                        using (var rd = new StreamReader(file.InputStream))
                        {
                            while (!rd.EndOfStream)
                            {
                                var splits = rd.ReadLine().Split(';');
                                if (splits[0].Length <= 0)
                                    continue;

                                votantes.Add(new Votante()
                                {
                                    Nome = splits[0].ToString(),
                                    CPF = splits[1].ToString(),
                                    Regional = Convert.ToInt32(splits[2].ToString()),
                                    NomeMae = splits[3]?.ToString(),
                                    DataNascimento = splits[4]?.ToString(),
                                    CriadoEm = DateTime.Now,
                                    AtualizadoEm = DateTime.Now
                                });
                            }
                        }
                        foreach (var votante in votantes)
                        {
                            BLL.Add(votante);
                        }
                    }
                }
                else throw new Exception("Nenhum arquivo localizado");
            }
            catch (Exception ex)
            {
                string mensagemErro = "Erro detalhe: " + ex.Message + " Possível causa: Arquivo ou os dados não estão corretos, arquivo deve ser.csv ";
                return Content($"<script language='javascript' type='text/javascript'>alert('{mensagemErro.Replace("\r", "").Replace("\n", "")}'); window.location.href = '{this.Request.Url.OriginalString}'</script>");
            }
            return Content("<script language='javascript' type='text/javascript'>alert('Importação concluída, novos votantes foram incluídos.');" +
                $"window.location.href = '{this.Request.Url.OriginalString}'</script>");
        }
    }
}