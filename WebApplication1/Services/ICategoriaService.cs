using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaViewModel>> GetCategorias();
        Task<CategoriaViewModel> GetCategoriaPorId(int id);
        Task<CategoriaViewModel> CriaCategoria(CategoriaViewModel categoriaVM);
        Task<bool> AtualizarCategoria(int id, CategoriaViewModel categoriaVM);
        Task<bool> DeletaCategoria(int id);
    }
}
