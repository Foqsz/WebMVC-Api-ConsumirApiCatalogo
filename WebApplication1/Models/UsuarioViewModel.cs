using System.ComponentModel.DataAnnotations;

namespace CategoriasMvc.Models
{
    public class UsuarioViewModel
    {
        [Required]
        public string userName { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
