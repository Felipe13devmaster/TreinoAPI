using System.Collections.Generic;

namespace MusicPlayer.Models
{
    public class GeneroMusical
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public ICollection<AlbumGenero> AlbunsGeneros { get; set; }
    }
}