using System.Collections.Generic;

namespace MusicPlayer.Models
{
    public class Artista
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public ICollection<AlbumArtista> AlbunsArtistas { get; set; }
    }
}