using PontoSys.Business.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PontoSys.Business.Models.Produtos
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> ObterProdutoPorFornecedor(Guid fornecedorId);

        Task<IEnumerable<Produto>> ObterProdutosFornecedores();

        Task<Produto> ObterProdutoFornecedor(Guid id);
    }
}
