using System;
using System.Collections.Generic;

namespace MusicPlayer.Models
{
    public class Musica
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public float DuracaoMinutos { get; set; }
        public ICollection<AlbumMusica> AlbunsMusicas { get; set; }
        public ICollection<PlaylistMusica> PlaylistsMusicas { get; set; }
    }
}