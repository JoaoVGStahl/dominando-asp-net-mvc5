using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoSys.AppMvc.Extensions
{
    public static class RazorExtensions
    {
        public static bool PermitirVer(this WebViewPage page, string claimName, string claimValue)
        {
            return CustomAuthorization.ValidarClaimsUsuario(claimName, claimValue); 
        }
        
        public static MvcHtmlString PermitirVer(this MvcHtmlString value, string claimName, string claimValue)
        {
            return CustomAuthorization.ValidarClaimsUsuario(claimName, claimValue) ? value : MvcHtmlString.Empty;
        }
        public static string ActionComPermissao(this UrlHelper url,string action, string controller, object routevalues,string claimName, string claimValue)
        {
            return CustomAuthorization.ValidarClaimsUsuario(claimName, claimValue) ? url.Action(action, controller, routevalues) : "";
        }
        public static string FormatarDocumento(this WebViewPage page,int tipoPessoa,string doc)
        {
            return tipoPessoa == 1
                ? Convert.ToUInt64(doc).ToString(@"000\.000\.000\-00")
                : Convert.ToUInt64(doc).ToString(@"00\.000\.000\/0000\-00");
        }
        public static bool ExibirNaUrl(this WebViewPage value, Guid Id)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            var urlTarget = urlHelper.Action("Edit", "Fornecedores", new { id = Id });
            var urlTarget2 = urlHelper.Action("ObterEndereco", "Fornecedores", new { id = Id });

            var urlEmUso = HttpContext.Current.Request.Path;

            return urlTarget == urlEmUso || urlTarget2 == urlEmUso;

        }
    }
}