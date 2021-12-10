using PontoSys.Business.Core.Notifications;
using PontoSys.Business.Core.Services;
using PontoSys.Business.Models.Fornecedores;
using PontoSys.Business.Models.Fornecedores.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PontoSys.Business.Models.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {
        private readonly IFornecedorRepository _fornecedorRepository;

        private readonly IEnderecoRepository _enderecoRepository;

        public FornecedorService(IFornecedorRepository fornecedorRepository,
                                 IEnderecoRepository enderecoRepository,
                                 INotification notifier) : base(notifier)
        {
            _fornecedorRepository = fornecedorRepository;
            _enderecoRepository = enderecoRepository;
        }

        public async Task Adicionar(Fornecedor fornecedor)
        {
            //Limitação EF 6
            fornecedor.Endereco.Id = fornecedor.Id;
            fornecedor.Endereco.Fornecedor = fornecedor;

            if (!ExecutarValiacao(new FornecedorValidation(), fornecedor)
                || !ExecutarValiacao(new EnderecoValidation(), fornecedor.Endereco)) return;

            if (await FornecedorExiste(fornecedor)) return;

            await _fornecedorRepository.Adicionar(fornecedor); 
        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            if (!ExecutarValiacao(new FornecedorValidation(), fornecedor)) return;

            if (await FornecedorExiste(fornecedor)) return;

            await _fornecedorRepository.Atualizar(fornecedor);
        }

        public async Task Remover(Guid id)
        {
            var fornecedor = await _fornecedorRepository.ObterFornecedorProdutosEndereco(id);

            if (fornecedor.Produtos.Any())
            {
                Notificar("O Fornecedor possui produtos cadastrados!");
                return;
            }

            if(fornecedor.Endereco != null)
            {
                await _enderecoRepository.Remover(fornecedor.Endereco.Id);
            }

            await _fornecedorRepository.Remover(id);
        }
        public async Task AtualizarEndereco(Endereco endereco)
        {
            if (!ExecutarValiacao(new EnderecoValidation(), endereco)) return;

            await _enderecoRepository.Atualizar(endereco);
        }

        private async Task<bool> FornecedorExiste(Fornecedor fornecedor)
        {
            var fornecedorAtual = await _fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id);

            if (!fornecedorAtual.Any()) return false;

            Notificar("Já Existe um fornecedor cadastrado com este documento!");

            return true;
        }

        public void Dispose()
        {
            _fornecedorRepository?.Dispose();
            _enderecoRepository?.Dispose(); 
        }
    }
}
