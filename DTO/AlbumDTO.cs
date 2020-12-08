using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicPlayer.DTO
{
    public class AlbumDTO
    {
        [Required(ErrorMessage = "É obrigatorio informar o nome do album.")]
        [MinLength(2, ErrorMessage = "Nome do album deve conter pelo menos dois caracteres.")]
        public string Nome { get; set; }

        [Range(1950, 2021, ErrorMessage = "É obrigatorio informar um ano de lançamento válido para o album.")]
        public int AnoLancamento { get; set; }

        [Required(ErrorMessage = "É obrigatorio informar este campo.")]
        [MinLength(1, ErrorMessage = "É obrigatorio informar no minimo um Id de artista.")]
        public List<int> ArtistaIds { get; set; }

        [Required(ErrorMessage = "É obrigatorio informar este campo.")]
        [MinLength(1, ErrorMessage = "É obrigatorio informar no minimo um Id de genero musical.")]
        public List<int> GeneroIds { get; set; }

        [Required(ErrorMessage = "É obrigatorio informar este campo (pode ser vazio).")]
        public List<int> MusicaIds { get; set; }
    }
}