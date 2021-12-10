using PontoSys.Business.Models.Fornecedores;
using System;
using System.Threading.Tasks;

namespace PontoSys.Business.Models.Services
{
    public interface IFornecedorService : IDisposable
    {
        Task Adicionar(Fornecedor fornecedor);

        Task Atualizar(Fornecedor fornecedor);

        Task Remover(Guid id);

        Task AtualizarEndereco(Endereco endereco);
    }
}
