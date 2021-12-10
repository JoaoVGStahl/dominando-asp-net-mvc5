using System.Collections.Generic;
using System.Linq;

namespace PontoSys.Business.Core.Notifications
{
    public class Notifier : INotification
    {
        private List<Notificacao> _notificacoes;

        public Notifier()
        {
            _notificacoes = new List<Notificacao>();
        }
        public void Handle(Notificacao notificacao)
        {
            _notificacoes.Add(notificacao);  
        }

        public List<Notificacao> ObterNotificacoes()
        {
            return _notificacoes;
        }

        public bool TemNotificacao()
        {
            return _notificacoes.Any();
        }
    }
}
