namespace MusicPlayer.Models
{
    public class AlbumMusica
    {
        public int AlbumId { get; set; }
        public Album Album { get; set; }
        public int MusicaId { get; set; }
        public Musica Musica { get; set; }
    }
}