using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPlayer.Data;
using MusicPlayer.DTO;
using MusicPlayer.Models;
using MusicPlayer.RESTful;

namespace MusicPlayer.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlaylistsController : ControllerBase
    {
        private readonly ApplicationDbContext _database;
        private Hateoas _hateoas;

        public PlaylistsController(ApplicationDbContext database)
        {
            _database = database;
            _hateoas = new Hateoas("localhost:5001/api/v1/Playlists");
            
            _hateoas.AddAction("GET_PLAYLIST", "GET");
            _hateoas.AddAction("EDIT_PLAYLISTS", "PATCH");
            _hateoas.AddAction("DELETE_PLAYLISTS", "DELETE");
        }

        [HttpGet]
        public IActionResult GetPlaylists()
        {
            var listaDePlaylists = _database.Playlists.
            Include(x => x.PlaylistsMusicas).ThenInclude(x => x.Musica).
            Include(x => x.UsuariosPlaylists).ThenInclude(x => x.Usuario).
            ToList();
            
            var playlistsHateoas = new List<Container<Playlist>>();

            foreach (var playlist in listaDePlaylists)
            {
                var playlistHateoas = new Container<Playlist>();
                playlistHateoas.ObjetoContainer = playlist;
                playlistHateoas.Links = _hateoas.GetActions(playlist.Id.ToString());
                
                playlistsHateoas.Add(playlistHateoas);
            }

            return Ok(playlistsHateoas);
        }

        [HttpGet("{id}")]
        public IActionResult GetPlaylists(int id)
        {
            try
            {
                var playlist = _database.Playlists.
                Include(x => x.PlaylistsMusicas).ThenInclude(x => x.Musica).
                Include(x => x.UsuariosPlaylists).ThenInclude(x => x.Usuario).
                First(playlist => playlist.Id == id);

                var playlistHateoas = new Container<Playlist>();

                playlistHateoas.ObjetoContainer = playlist;
                playlistHateoas.Links = _hateoas.GetActions(playlist.Id.ToString());

                return Ok(playlistHateoas);
            }
            catch (InvalidOperationException)
            {
                Response.StatusCode = 404;
                return new ObjectResult(new{msg = "Nada encontrado."});
            }
        }

        [HttpPost]
        public IActionResult NovoPlaylist([FromBody] PlaylistDTO playlistDTO)
        {
            if (ModelState.IsValid)
            {
                var playlist = new Playlist();
                playlist.Nome = playlistDTO.Nome;
                playlist.DuracaoMinutos = playlistDTO.DuracaoMinutos;

                _database.Playlists.Add(playlist);
                _database.SaveChanges();

                var playlistsDoBanco = _database.Playlists.ToList();

                var playlistMusica = new PlaylistMusica();

                foreach (var musicaId in playlistDTO.MusicaIds)
                {
                    playlistMusica.PlaylistId = playlistsDoBanco.Last().Id;
                    playlistMusica.MusicaId = musicaId;

                    _database.PlaylistsMusicas.Add(playlistMusica);
                    _database.SaveChanges();
                }

                var usuarioPlaylist = new UsuarioPlaylist();

                foreach (var usuarioId in playlistDTO.UsuarioIds)
                {
                    usuarioPlaylist.PlaylistId = playlistsDoBanco.Last().Id;
                    usuarioPlaylist.Usuarioid = usuarioId;

                    _database.UsuariosPlaylists.Add(usuarioPlaylist);
                    _database.SaveChanges();
                }

                Response.StatusCode = 201;
                return new ObjectResult(new{info = "Novo playlist adicionado."});
            }

            Response.StatusCode = 401;
            return new ObjectResult(new{info = "Erro ao adicionar playlist."});
        }

        [HttpPatch]
        public IActionResult EditarPlaylist([FromBody] Playlist playlist)
        {
            if (playlist.Id < 1)
            {
                Response.StatusCode = 400;

                return new ObjectResult(new{msg = "Id inválido."});
            }
            else
            {
                try
                {
                    var playlistEditado = _database.Playlists.First(registro => registro.Id == playlist.Id);

                    playlistEditado.Nome = playlist.Nome != null ? playlist.Nome : playlistEditado.Nome;
                    playlistEditado.DuracaoMinutos = playlist.DuracaoMinutos != 0 ? playlist.DuracaoMinutos : playlistEditado.DuracaoMinutos;

                    _database.SaveChanges();

                    return Ok();
                }
                catch (InvalidOperationException)
                {
                    Response.StatusCode = 400;

                    return new ObjectResult(new{msg = "playlist não encontrado."});
                }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult ExcluirPlaylist(int id)
        {
            try
            {
                var playlist = _database.Playlists.First(playlist => playlist.Id == id);

                _database.Playlists.Remove(playlist);
                _database.SaveChanges();

            return Ok();
            }
            catch (InvalidOperationException)
            {
                Response.StatusCode = 404;

                return new ObjectResult(new{msg = "Id inválido."});
            }
        }
        
    }
}