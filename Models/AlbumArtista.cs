namespace MusicPlayer.Models
{
    public class AlbumArtista
    {
        public int AlbumId { get; set; }
        public Album Album { get; set; }
        public int ArtistaId { get; set; }
        public Artista Artista { get; set; }
    }
}