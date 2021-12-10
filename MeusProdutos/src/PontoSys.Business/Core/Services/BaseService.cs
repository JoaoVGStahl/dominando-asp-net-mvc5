using FluentValidation;
using FluentValidation.Results;
using PontoSys.Business.Core.Models;
using PontoSys.Business.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PontoSys.Business.Core.Services
{
    public abstract class BaseService
    {
        private readonly INotification _notifier;

        public BaseService(INotification notifier)
        {
            _notifier = notifier;
        }

        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notificar(error.ErrorMessage);
            }
        }
        protected void Notificar(string mensagem)
        {
            _notifier.Handle(new Notificacao(mensagem));
        }
        protected bool ExecutarValiacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
        {
            var validator = validacao.Validate(entidade);

            if (validator.IsValid) return true;

            Notificar(validator);

            return false;
        }
    }
}
