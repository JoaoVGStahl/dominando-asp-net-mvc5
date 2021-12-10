using PontoSys.Business.Models.Fornecedores;
using System;
using System.Threading.Tasks;
using PontoSys.Infra.Data.Context;

namespace PontoSys.Infra.Data.Repository
{
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(AppMvcContext context) : base(context)
        {

        }
        public async Task<Endereco> ObterEnderecoFornecedor(Guid fornecedorId)
        {
            return await ObterPorId(fornecedorId);
        }
    }
}
