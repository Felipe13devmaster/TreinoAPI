using System.Collections.Generic;

namespace MusicPlayer.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int AnoLancamento { get; set; }
        public ICollection<AlbumArtista> AlbunsArtistas { get; set; }
        public ICollection<AlbumMusica> AlbunsMusicas { get; set; }
        public ICollection<AlbumGenero> AlbunsGeneros { get; set; }
    }
}