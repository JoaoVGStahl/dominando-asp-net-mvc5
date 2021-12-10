using PontoSys.Business.Core.Data;
using System;
using System.Threading.Tasks;

namespace PontoSys.Business.Models.Fornecedores
{
    public interface IEnderecoRepository : IRepository<Endereco>
    {
        Task<Endereco> ObterEnderecoFornecedor(Guid fornecedorId);  
    }
}
