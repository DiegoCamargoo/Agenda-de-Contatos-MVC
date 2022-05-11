using ControleDeContatos.Filters;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;


namespace ControleDeContatos.Controllers
{
    [PaginaParaUsuarioLogado]
    
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }
        public IActionResult Index()
        {
            List<UsuarioModel> usuarios = _usuarioRepositorio.BuscarTodos();

            return View(usuarios);
        }
        public IActionResult Criar()
        {
            return View();
        }
        public IActionResult Editar(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);
            return View();
        }
        public IActionResult Deletar(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);
            return View(usuario);
        }

        public IActionResult DeletarConfirmacao(int id)
        {
            try
            {
                bool deletado = _usuarioRepositorio.Deletar(id);
                if (deletado)
                {
                    TempData["MensagemSucesso"] = "Usuário apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemSucesso"] = "Ops, não conseguimos apagar seu usuário!";
                }

                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemSucesso"] = $"Ops, não conseguimos apagar seu usuário, mais detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public IActionResult Criar(UsuarioModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                { 
                  usuario = _usuarioRepositorio.Adicionar(usuario);
                    TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso";
                    return RedirectToAction("Index");
                }

                return View(usuario);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não foi possivel cadastrar seu usuario, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public IActionResult Editar(UsuarioSemSenhaModel usuarioSemSenhaModel)
        {
            try
            {
                UsuarioModel usuario = null;

                if (ModelState.IsValid) 
                {
                    usuario = new UsuarioModel()
                    {
                        Id = usuarioSemSenhaModel.Id,
                        Nome = usuarioSemSenhaModel.Nome,
                        Login = usuarioSemSenhaModel.Login,
                        Email = usuarioSemSenhaModel.Email,
                        Perfil = (Enums.PerfilEnum)usuarioSemSenhaModel.Perfil
                    };

                    usuario = _usuarioRepositorio.Atualizar(usuario);
                    TempData["MensagemSucesso"] = "Usúario atualizado com sucesso";
                    return RedirectToAction("Index");

                }
                return View(usuario);
            }
            catch (Exception erro)
            {

                TempData["MensagemErro"] = $"Ops, não foi possivel atualizar seu usúario tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }


        }
    }
}
