using System.Collections.Generic;

namespace MusicPlayer.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public float DuracaoMinutos { get; set; }
        public ICollection<PlaylistMusica> PlaylistsMusicas { get; set; }
        public ICollection<UsuarioPlaylist> UsuariosPlaylists { get; set; }
    }
}