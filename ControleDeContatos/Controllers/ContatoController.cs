using ControleDeContatos.Filters;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    [PaginaParaUsuarioLogado]
    public class ContatoController : Controller
    {
        private readonly IContatoRepositorio _contatoRepositorio;
        public ContatoController(IContatoRepositorio contatoRepositorio)
        {
            _contatoRepositorio = contatoRepositorio;
        }
        
        public IActionResult Index()
        {
          List<ContatoModel> contatos = _contatoRepositorio.BuscarTodos();
            return View(contatos);
        }
        public IActionResult Criar()
        {
            return View();
        }
        public IActionResult Editar(int id)
        {
            ContatoModel contato =  _contatoRepositorio.ListarPorId(id);
            return View(contato);
        }

        public IActionResult DeletarConfirmacao(int id)
        {
            try
            {
                bool deletado = _contatoRepositorio.Deletar(id);
                if (deletado)
                {
                    TempData["MensagemSucesso"] = "Contato apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemSucesso"] = "Ops, não conseguimos apagar seu contato!";
                }

                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemSucesso"] = $"Ops, não conseguimos apagar seu contato, mais detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
            
        }

        public IActionResult Deletar(int id)
        {
            ContatoModel contato = _contatoRepositorio.ListarPorId(id);
            return View(contato);
        }

        [HttpPost]
        public IActionResult Criar(ContatoModel contato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _contatoRepositorio.Adicionar(contato);
                    TempData["MensagemSucesso"] = "Contato cadastrado com sucesso";
                    return RedirectToAction("Index");
                }

                return View(contato);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não foi possivel cadastrar seu contato, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }
        
        [HttpPost]
         public IActionResult Atualizar(ContatoModel contato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _contatoRepositorio.Atualizar(contato);
                    TempData["MensagemSucesso"] = "Contato atualizado com sucesso";
                    return RedirectToAction("Index");
                   
                }
                return View("Editar", contato);
            }
            catch(Exception erro)
            {

                TempData["MensagemErro"] = $"Ops, não foi possivel atualizar seu contato, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
            

        }


    }

}
