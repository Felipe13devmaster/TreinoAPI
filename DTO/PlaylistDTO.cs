using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicPlayer.DTO
{
    public class PlaylistDTO
    {
        [Required(ErrorMessage = "É obrigatorio informar o nome da playlist.")]
        [MinLength(2, ErrorMessage = "Nome da playlist deve conter pelo menos dois caracteres.")]
        public string Nome { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "É obrigatorio informar uma duração de playlist em minutos válida.")]
        public float DuracaoMinutos { get; set; }

        [Required(ErrorMessage = "É obrigatorio informar este campo.")]
        [MinLength(1, ErrorMessage = "É obrigatorio informar no minimo um Id de música.")]
        public List<int> MusicaIds { get; set; }

        [Required(ErrorMessage = "É obrigatorio informar este campo.")]
        [MinLength(1, ErrorMessage = "É obrigatorio informar no minimo um Id de usuário.")]
        public List<int> UsuarioIds { get; set; }
    }
}