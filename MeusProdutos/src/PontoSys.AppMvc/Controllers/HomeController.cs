using PontoSys.AppMvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoSys.AppMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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
        [Route("erro/{id:length(3,3)}")]
        public ActionResult Errors(int id)
        {
            var modelErro = new ErrorViewModel();
            switch (id)
            {
                case 500:
                    modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou entre em contato com a equipe de suporte!";
                    modelErro.Titulo = "Ocorreu um erro!";
                    modelErro.ErrorCode = id;
                    break;
                case 404:
                    modelErro.Mensagem = "A página solicitado não foi encontrada!";
                    modelErro.Titulo = "Ops! agina não encontrada!";
                    modelErro.ErrorCode = id;
                    break;
                case 403:
                    modelErro.Mensagem = "Você não tem premissão para acessa esta página!";
                    modelErro.Titulo = "Acesso negado!";
                    modelErro.ErrorCode = id;
                    break;
                default:
                    return new HttpStatusCodeResult(500);
            }

            return View("Error", modelErro);
        }
    }
}