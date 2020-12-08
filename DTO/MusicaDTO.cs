using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicPlayer.DTO
{
    public class MusicaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "É obrigatorio informar o nome da música.")]
        [MinLength(2, ErrorMessage = "Nome da música deve conter pelo menos dois caracteres.")]
        public string Nome { get; set; }

        [Range(1, 20, ErrorMessage = "É obrigatorio informar uma duração de música em minutos válida.")]
        public float DuracaoMinutos { get; set; }

        [Required(ErrorMessage = "É obrigatorio informar este campo (pode ser vazio).")]
        public List<int> AlbumIds { get; set; }

        [Required(ErrorMessage = "É obrigatorio informar este campo (pode ser vazio).")]
        public List<int> PlaylistIds { get; set; }
    }
}