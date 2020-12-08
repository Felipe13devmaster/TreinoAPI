using System.ComponentModel.DataAnnotations;

namespace MusicPlayer.DTO
{
    public class GeneroMusicalDTO
    {
        [Required(ErrorMessage = "É obrigatorio informar o nome do gênero.")]
        [MinLength(3, ErrorMessage = "Nome do gênero deve conter pelo menos três caracteres.")]
        public string Nome { get; set; }
    }
}