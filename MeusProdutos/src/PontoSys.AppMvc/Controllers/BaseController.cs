using PontoSys.Business.Core.Notifications;
using System.Web.Mvc;

namespace PontoSys.AppMvc.Controllers
{
    public class BaseController : Controller
    {
        private readonly INotification _notifier;
        public BaseController(INotification notifier)
        {
            _notifier = notifier;
        }
        public bool OperacaoValida()
        {
            if (!_notifier.TemNotificacao()) return true;

            var notificacoes = _notifier.ObterNotificacoes();
            notificacoes.ForEach(ex => ViewData.ModelState.AddModelError(string.Empty, ex.Mensagem));
            return false;
        }
    }
}