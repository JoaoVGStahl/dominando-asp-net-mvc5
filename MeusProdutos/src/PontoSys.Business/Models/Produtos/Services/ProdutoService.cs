using PontoSys.Business.Core.Notifications;
using PontoSys.Business.Core.Services;
using PontoSys.Business.Models.Produtos.Validations;
using System;
using System.Threading.Tasks;

namespace PontoSys.Business.Models.Produtos.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        public ProdutoService(IProdutoRepository produtoRepository, 
                              INotification notifier) : base(notifier)
        {
            _produtoRepository = produtoRepository;
        }
        public async Task Adicionar(Produto produto)
        {
            if (!ExecutarValiacao(new ProdutoValidations(), produto)) return;

            await _produtoRepository.Adicionar(produto);
        }

        public async Task Atualizar(Produto produto)
        {
            if (!ExecutarValiacao(new ProdutoValidations(), produto)) return;

            await _produtoRepository.Atualizar(produto);
        }

        public async Task Remover(Guid id)
        {
            await _produtoRepository.Remover(id);   
        }

        public void Dispose()
        {
            _produtoRepository?.Dispose();
        }
    }
}
