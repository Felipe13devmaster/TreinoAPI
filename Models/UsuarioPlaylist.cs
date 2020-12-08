namespace MusicPlayer.Models
{
    public class UsuarioPlaylist
    {
        public int Usuarioid { get; set; }
        public Usuario Usuario { get; set; }
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}