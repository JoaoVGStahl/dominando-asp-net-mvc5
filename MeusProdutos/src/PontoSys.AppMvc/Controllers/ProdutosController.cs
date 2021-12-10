using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using PontoSys.AppMvc.ViewModels;
using PontoSys.Business.Models.Produtos;
using PontoSys.Business.Models.Produtos.Services;
using AutoMapper;
using System.Collections.Generic;
using PontoSys.Business.Models.Fornecedores;
using System.Web;
using System.IO;
using PontoSys.Business.Core.Notifications;
using PontoSys.AppMvc.Extensions;

namespace PontoSys.AppMvc.Controllers
{
    [Authorize]
    public class ProdutosController : BaseController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly INotification _notifier;
        private readonly IMapper _mapper;
        public ProdutosController(IProdutoRepository produtoRepository,
                                  IProdutoService produtoService,
                                  IFornecedorRepository fornecedorRepository,
                                  INotification notifier,
                                  IMapper mapper) : base(notifier)
        {
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
            _fornecedorRepository = fornecedorRepository;
            _notifier = notifier;
            _mapper = mapper;
        }
        // GET: Produtos
        [AllowAnonymous]
        [Route("lista-produtos")]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores()));
        }

        // GET: Produtos/Details/5
        [AllowAnonymous]
        [Route("detalhes/{id:guid}")]
        [HttpGet]
        public async Task<ActionResult> Details(Guid id)
        {
            var produtoVM = await ObterProduto(id);

            if(produtoVM == null)
            {
                return HttpNotFound();
            }

            return View(produtoVM);
        }


        // GET: Produtos/Create
        [Claim("Produto", "Criar")]
        [Route("novo-produto")]
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var produtosVM = await PopularFornecedores(new ProdutoViewModel());

            return View(produtosVM);
        }
        // POST : Produtos/Create
        [Claim("Produto", "Criar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("novo-produto")]
        public async Task<ActionResult> Create(ProdutoViewModel produtoVM)
        {

            if (!ModelState.IsValid)
            {
                return View(produtoVM);
            }

            var imgPrefix = Guid.NewGuid() + "-";

            if(!UploadImagem(produtoVM.ImagemUpload, imgPrefix))
            {
                return View(produtoVM);
            }

            produtoVM.Imagem = imgPrefix + produtoVM.ImagemUpload.FileName;

            await _produtoRepository.Adicionar(_mapper.Map<Produto>(produtoVM));

            return RedirectToAction("Index");
        }

        // GET: Produtos/Edit/5
        [Claim("Produto", "Editar")]
        [Route("editar-produto/{id:guid}")]
        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            var produtoVM = await ObterProduto(id);

            if (produtoVM == null)
            {
                return HttpNotFound();
            }
            return View(produtoVM);
        }

        // POST: Produtos/Edit/5
        [Claim("Produto", "Editar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editar-produto/{id:guid}")]
        public async Task<ActionResult> Edit(ProdutoViewModel produtoVM)
        {
            if (!ModelState.IsValid)
            {
                return View(produtoVM);
            }

            var produtoatualizacao = await ObterProduto(produtoVM.Id);

            produtoVM.Imagem = produtoatualizacao.Imagem;

            if(produtoVM.ImagemUpload != null)
            {
                var imgPrefix = Guid.NewGuid() + "-";

                if (!UploadImagem(produtoVM.ImagemUpload, imgPrefix))
                {
                    return View(produtoVM);
                }

                produtoatualizacao.Imagem = imgPrefix + produtoVM.ImagemUpload.FileName;
            }

            produtoatualizacao.Nome = produtoVM.Nome;
            produtoatualizacao.Descricao = produtoVM.Descricao;
            produtoatualizacao.Valor = produtoVM.Valor;
            produtoatualizacao.Ativo = produtoVM.Ativo;
            produtoatualizacao.FornecedorId = produtoVM.FornecedorId;
            produtoatualizacao.Fornecedor = produtoVM.Fornecedor;

            await _produtoService.Atualizar(_mapper.Map<Produto>(produtoatualizacao));

            return RedirectToAction("Index");
        }

        // GET: Produtos/Delete/5
        [Claim("Produto", "Excluir")]
        [Route("excluir-produto/{id:guid}")]
        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            var produtoVM = await ObterProduto(id);

            if (produtoVM == null)
            {
                return HttpNotFound();
            }
            return View(produtoVM);
        }

        // POST: Produtos/Delete/5
        [Claim("Produto", "Excluir")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("excluir-produto/{id:guid}")]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var produtoVM = await ObterProduto(id);

            if (produtoVM == null) return HttpNotFound();

            await _produtoService.Remover(id);

            return RedirectToAction("Index");
        }
        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            var produto = _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            return produto;
        }
        private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produto)
        {
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            
            return produto;
        }

        private bool UploadImagem(HttpPostedFileBase img , string imgPrefix)
        {
            if(img == null || img.ContentLength <= 0)
            {
                ModelState.AddModelError(string.Empty, "Imagem em formato inválido!");
                return false;
            }

            var path = Path.Combine(HttpContext.Server.MapPath("~/imagens"), imgPrefix + img.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return false;
            }

            img.SaveAs(path);
            return true;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _produtoRepository?.Dispose();
                _produtoService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
