using Microsoft.EntityFrameworkCore;
using MusicPlayer.Models;

namespace MusicPlayer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Album> Albuns { get; set; }
        public DbSet<Artista> Artistas { get; set; }
        public DbSet<GeneroMusical> GenerosMusicais { get; set; }
        public DbSet<Musica> Musicas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<AlbumArtista> AlbunsArtistas { get; set; }
        public DbSet<AlbumGenero> AlbunsGeneros { get; set; }
        public DbSet<AlbumMusica> AlbunsMusicas { get; set; }
        public DbSet<PlaylistMusica> PlaylistsMusicas { get; set; }
        public DbSet<UsuarioPlaylist> UsuariosPlaylists { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AlbumArtista>().HasKey(chaveComposta => new{chaveComposta.AlbumId, chaveComposta.ArtistaId});
            builder.Entity<AlbumGenero>().HasKey(chaveComposta => new{chaveComposta.AlbumId, chaveComposta.GeneroMusicalId});
            builder.Entity<AlbumMusica>().HasKey(chaveComposta => new{chaveComposta.AlbumId, chaveComposta.MusicaId});
            builder.Entity<PlaylistMusica>().HasKey(chaveComposta => new{chaveComposta.PlaylistId, chaveComposta.MusicaId});
            builder.Entity<UsuarioPlaylist>().HasKey(chaveComposta => new{chaveComposta.Usuarioid, chaveComposta.PlaylistId});
        }
    }
}