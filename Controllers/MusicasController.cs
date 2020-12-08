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
    public class MusicasController : ControllerBase
    {
     
        private readonly ApplicationDbContext _database;
        private Hateoas _hateoas;

        public MusicasController(ApplicationDbContext database)
        {
            _database = database;
            _hateoas = new Hateoas("localhost:5001/api/v1/Musicas");
            
            _hateoas.AddAction("GET_MUSICA", "GET");
            _hateoas.AddAction("EDIT_MUSICAS", "PATCH");
            _hateoas.AddAction("DELETE_MUSICAS", "DELETE");
        }

        [HttpGet]
        public IActionResult GetMusica()
        {
            var listaDeMusicas = _database.Musicas.
            Include(x => x.AlbunsMusicas).ThenInclude(x => x.Album).
            Include(x => x.PlaylistsMusicas).ThenInclude(x => x.Playlist).
            ToList();

            var musicasHateoas = new List<Container<Musica>>();

            foreach (var musica in listaDeMusicas)
            {
                var musicaHateoas = new Container<Musica>();
                musicaHateoas.ObjetoContainer = musica;
                musicaHateoas.Links = _hateoas.GetActions(musica.Id.ToString());
                
                musicasHateoas.Add(musicaHateoas);
            }

            return Ok(musicasHateoas);
        }

        [HttpGet("{id}")]
        public IActionResult GetMusica(int id)
        {
            try
            {
                var musica = _database.Musicas.
                Include(x => x.AlbunsMusicas).ThenInclude(x => x.Album).
                Include(x => x.PlaylistsMusicas).ThenInclude(x => x.Playlist).
                First(musica => musica.Id == id);

                var musicaHateoas = new Container<Musica>();

                musicaHateoas.ObjetoContainer = musica;
                musicaHateoas.Links = _hateoas.GetActions(musica.Id.ToString());

                return Ok(musicaHateoas);
            }
            catch (InvalidOperationException)
            {
                Response.StatusCode = 404;
                return new ObjectResult(new{msg = "Nada encontrado."});
            }
        }

        [HttpPost]
        public IActionResult NovoMusica([FromBody] MusicaDTO musicaDTO)
        {
            if (ModelState.IsValid)
            {
                var musica = new Musica();
                musica.Nome = musicaDTO.Nome;
                musica.DuracaoMinutos = musicaDTO.DuracaoMinutos;

                _database.Musicas.Add(musica);
                _database.SaveChanges();

                var musicasDoBanco = _database.Musicas.ToList();

                var albumMusica = new AlbumMusica();

                foreach (var albumId in musicaDTO.AlbumIds)
                {
                    albumMusica.MusicaId = musicasDoBanco.Last().Id;
                    albumMusica.AlbumId = albumId;

                    _database.AlbunsMusicas.Add(albumMusica);
                    _database.SaveChanges();
                }

                var playlistMusica = new PlaylistMusica();

                foreach (var playlistId in musicaDTO.PlaylistIds)
                {
                    playlistMusica.MusicaId = musicasDoBanco.Last().Id;
                    playlistMusica.PlaylistId = playlistId;
                    
                    var playlist = _database.Playlists.First(x => x.Id == playlistId);
                    playlist.DuracaoMinutos += musicaDTO.DuracaoMinutos;

                    _database.PlaylistsMusicas.Add(playlistMusica);
                    _database.SaveChanges();
                }

                Response.StatusCode = 201;
                return new ObjectResult(new{info = "Nova música adicionada."});
            }

            Response.StatusCode = 401;
            return new ObjectResult(new{info = "Erro ao adicionar música."});
        }

        [HttpPatch]
        public IActionResult EditarMusica([FromBody] Musica musica)
        {
            if (musica.Id < 1)
            {
                Response.StatusCode = 400;

                return new ObjectResult(new{msg = "Id inválido."});
            }
            else
            {
                try
                {
                    var musicaEditado = _database.Musicas.First(registro => registro.Id == musica.Id);

                    musicaEditado.Nome = musica.Nome != null ? musica.Nome : musicaEditado.Nome;
                    musicaEditado.DuracaoMinutos = musica.DuracaoMinutos != 0 ? musica.DuracaoMinutos : musicaEditado.DuracaoMinutos;

                    _database.SaveChanges();

                    return Ok();
                }
                catch (InvalidOperationException)
                {
                    Response.StatusCode = 400;

                    return new ObjectResult(new{msg = "Música não encontrada."});
                }
            }   
        }

        [HttpDelete("{id}")]
        public IActionResult ExcluirMusica(int id)
        {
            try
            {
                var musica = _database.Musicas.First(musica => musica.Id == id);

                _database.Musicas.Remove(musica);
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