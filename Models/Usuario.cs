using System.Collections.Generic;

namespace MusicPlayer.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public ICollection<UsuarioPlaylist> UsuariosPlaylists { get; set; }
    }
}