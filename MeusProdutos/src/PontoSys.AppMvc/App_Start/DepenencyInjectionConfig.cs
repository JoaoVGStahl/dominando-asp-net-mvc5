using PontoSys.Business.Core.Notifications;
using PontoSys.Business.Models.Fornecedores;
using PontoSys.Business.Models.Produtos;
using PontoSys.Business.Models.Produtos.Services;
using PontoSys.Business.Models.Services;
using PontoSys.Infra.Data.Context;
using PontoSys.Infra.Data.Repository;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace PontoSys.AppMvc.App_Start
{
    public class DepenencyInjectionConfig
    {
        public static void RegisterDIContainer()
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainer(Container container)
        {

            //Lifestyle.Singleton 
            //Uma instância para todas a aplicação

            //Lifestyle.Transient
            //Cria uma instância para todas a Inejação

            //Lifestyle.Scoped
            //Uma Unicao Instância para cada request

            container.Register<AppMvcContext>(Lifestyle.Scoped);
            container.Register<IProdutoRepository, ProdutoRepository>(Lifestyle.Scoped);
            container.Register<IProdutoService, ProdutoService>(Lifestyle.Scoped);
            container.Register<IFornecedorRepository, FornecedorRepository>(Lifestyle.Scoped);  
            container.Register<IEnderecoRepository, EnderecoRepository>(Lifestyle.Scoped);
            container.Register<IFornecedorService, FornecedorService>(Lifestyle.Scoped);
            container.Register<INotification, Notifier>(Lifestyle.Scoped);

            container.RegisterSingleton(() => AutoMapperConfig.GetMapperConfiguration().CreateMapper(container.GetInstance));
        }
    }
}