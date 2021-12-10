using PontoSys.Business.Models.Fornecedores;
using PontoSys.Infra.Data.Context;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace PontoSys.Infra.Data.Repository
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(AppMvcContext context) : base(context)
        {

        }
        public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
        {
            var fornecedor = await Db.Fornecedores.AsNoTracking()
                .Include(f => f.Endereco)
                .FirstOrDefaultAsync(f => f.Id == id);
            return fornecedor;
        }

        public async Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id)
        {
            return await Db.Fornecedores.AsNoTracking()
                .Include(f => f.Endereco)
                .Include(f => f.Produtos)
                .FirstOrDefaultAsync( f=> f.Id == id);
        }

        public override async Task Remover(Guid id)
        {
            var fornecedor =  await ObterPorId(id);
            fornecedor.Ativo = false;

            await Atualizar(fornecedor);
        }
    }
}
