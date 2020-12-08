using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicPlayer.DTO
{
    public class UsuarioDTO
    {
        [Required(ErrorMessage = "É obrigatorio informar o nome do usuário.")]
        [MinLength(2, ErrorMessage = "Nome do usuário deve conter pelo menos dois caracteres.")]
        public string Nome { get; set; }

        [Range(1, 110, ErrorMessage = "É obrigatorio informar uma idade válida.")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "É obrigatorio informar este campo (pode ser vazio).")]
        public List<int> PlaylistIds { get; set; }
    }
}