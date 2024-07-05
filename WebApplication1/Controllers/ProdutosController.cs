using CategoriasMvc.Models;
using CategoriasMvc.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace CategoriasMvc.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly IProdutoService _produtoService;
        private readonly ICategoriaService _categoriaService;
        private string token = string.Empty;

        public ProdutosController(IProdutoService produtoService, ICategoriaService categoriaService)
        {
            _produtoService = produtoService;
            _categoriaService = categoriaService;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<ProdutoViewModel>>> Index()
        {
            //extrair o token do cookie
            var result = await _produtoService.GetProdutos(ObtemTokenJwt());

            if (result is null)
            {
                return View("Error");
            }

            return View(result);
        }

        private string ObtemTokenJwt()
        {
            if (HttpContext.Request.Cookies.ContainsKey("X-Acess-Token"))
                token = HttpContext.Request.Cookies["X-Acess-Token"].ToString();

            return token;
        }
    }
}
