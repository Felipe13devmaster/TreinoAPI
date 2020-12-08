namespace MusicPlayer.Models
{
    public class AlbumGenero
    {
        public int AlbumId { get; set; }
        public Album Album { get; set; }
        public int GeneroMusicalId { get; set; }
        public GeneroMusical GeneroMusical { get; set; }
    }
}