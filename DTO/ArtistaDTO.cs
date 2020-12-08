using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicPlayer.DTO
{
    public class ArtistaDTO
    {
        [Required(ErrorMessage = "É obrigatorio informar o nome do artista.")]
        [MinLength(2, ErrorMessage = "Nome do artista deve conter pelo menos dois caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "É obrigatorio informar este campo (pode ser vazio).")]
        public List<int> AlbumIds { get; set; }
    }
}