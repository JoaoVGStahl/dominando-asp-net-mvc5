using AutoMapper;
using PontoSys.AppMvc.Extensions;
using PontoSys.AppMvc.ViewModels;
using PontoSys.Business.Core.Notifications;
using PontoSys.Business.Models.Fornecedores;
using PontoSys.Business.Models.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PontoSys.AppMvc.Controllers
{
    [Authorize]
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly INotification _notifier;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository fornecedorRepository,
                                      IFornecedorService fornecedorService,
                                      INotification notifier,
                                      IMapper mapper) : base(notifier)
        {
            _fornecedorRepository = fornecedorRepository;
            _fornecedorService = fornecedorService;
            _notifier = notifier;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("lista-fornecedores")]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos()));
        }

        [AllowAnonymous]
        [Route("detalhes-fornecedor/{id:guid}")]
        public async Task<ActionResult> Details(Guid id)
        {
            var fornecedorVM = await ObterFornecedorEndereco(id);

            if (fornecedorVM == null) return HttpNotFound();

            return View(fornecedorVM);
        }

        [Claim("Fornecedor","Criar")]
        [Route("novo-fornecedor")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Claim("Fornecedor", "Criar")]
        [Route("novo-fornecedor")]
        [HttpPost]
        public async Task<ActionResult> Create(FornecedorViewModel fornecedorVM)
        {
            if (!ModelState.IsValid) return View(fornecedorVM);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorVM);

            await _fornecedorService.Adicionar(fornecedor);

            if (!OperacaoValida()) return View(fornecedorVM);

            return RedirectToAction("Index");
        }

        [Claim("Fornecedor", "Editar")]
        [Route("editar-fornecedor/{id:guid}")]
        [HttpGet]

        public async Task<ActionResult> Edit(Guid id)
        {
            var fornecedorVM = await ObterFornecedorProdutosEndereco(id);

            if (fornecedorVM == null)
            {
                return HttpNotFound();
            }
            return View(fornecedorVM);
        }

        [Claim("Fornecedor", "Editar")]
        [Route("editar-fornecedor/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, FornecedorViewModel fornecedorVM)
        {
            if (id != fornecedorVM.Id) return HttpNotFound();

            if (!ModelState.IsValid) return View(fornecedorVM);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorVM);

            await _fornecedorService.Atualizar(fornecedor);

            if (!OperacaoValida()) return View(fornecedorVM);

            return RedirectToAction("Index");

        }

        [Claim("Fornecedor", "Excluir")]
        [Route("deletar-fornecedor/{id:guid}")]
        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            var FornecedorVM = await ObterFornecedorEndereco(id);

            if (FornecedorVM == null)
            {
                return HttpNotFound();
            }
            return View(FornecedorVM);
        }

        [Claim("Fornecedor", "Excluir")]
        [Authorize]
        [Route("deletar-fornecedor/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedorVM = await ObterFornecedorEndereco(id);

            if (fornecedorVM == null) return HttpNotFound();

            await _fornecedorService.Remover(id);

            if (!OperacaoValida()) return View(fornecedorVM);

            return RedirectToAction("Index");
        }

        [Route("obter-endereco-fornecedor/{id:guid}")]
        public async Task<ActionResult> ObterEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if (fornecedor == null)
            {
                return HttpNotFound();
            }
            return PartialView("_DetalhesEndereco", fornecedor);
        }

        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        [HttpGet]
        public async Task<ActionResult> AtualizarEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if (fornecedor == null)
            {
                return HttpNotFound();
            }
            return PartialView("_AtualizarEndereco", new FornecedorViewModel { Endereco = fornecedor.Endereco });
        }

        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        [HttpPost]
        public async Task<ActionResult> AtualizarEndereco(FornecedorViewModel fornecedorVM)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("Documento");

            if (!ModelState.IsValid) return PartialView("_AtualizarEndereco", fornecedorVM);

            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(fornecedorVM.Endereco));

            var url = Url.Action("ObterEndereco", "Fornecedores", new { id = fornecedorVM.Endereco.FornecedorId });
            return Json(new { success = true, url });
        }

        private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }

        private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
        }
    }
}