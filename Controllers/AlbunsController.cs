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
    public class AlbunsController : ControllerBase
    {
        private readonly ApplicationDbContext _database;
        private Hateoas _hateoas;

        public AlbunsController(ApplicationDbContext database)
        {
            _database = database;
            _hateoas = new Hateoas("localhost:5001/api/v1/Albuns");
            
            _hateoas.AddAction("GET_ALBUM", "GET");
            _hateoas.AddAction("EDIT_ALBUNS", "PATCH");
            _hateoas.AddAction("DELETE_ALBUNS", "DELETE");
        }

        [HttpGet]
        public IActionResult GetAlbum()
        {
            var listaDeAlbuns = _database.Albuns.
            Include(x => x.AlbunsArtistas).
            Include(x => x.AlbunsGeneros).
            Include(x => x.AlbunsMusicas).
            ToList();

            var albunsHateoas = new List<Container<Album>>();

            foreach (var album in listaDeAlbuns)
            {
                var albumHateoas = new Container<Album>();
                albumHateoas.ObjetoContainer = album;
                albumHateoas.Links = _hateoas.GetActions(album.Id.ToString());

                albunsHateoas.Add(albumHateoas);
            }

            return Ok(albunsHateoas);
        }

        [HttpGet("{id}")]
        public IActionResult GetAlbum(int id)
        {
            try
            {
                var album = _database.Albuns.
                Include(x => x.AlbunsArtistas).ThenInclude(x => x.Artista).
                Include(x => x.AlbunsGeneros).ThenInclude(x => x.GeneroMusical).
                Include(x => x.AlbunsMusicas).ThenInclude(x => x.Musica).
                First(album => album.Id == id);

                var albumHateoas = new Container<Album>();

                albumHateoas.ObjetoContainer = album;
                albumHateoas.Links = _hateoas.GetActions(album.Id.ToString());

                return Ok(albumHateoas);
            }
            catch (InvalidOperationException)
            {
                Response.StatusCode = 404;
                return new ObjectResult(new{msg = "Nada encontrado."});
            }
            
        }

        [HttpPost]
        public IActionResult NovoAlbum([FromBody] AlbumDTO albumDTO)
        {
            if (ModelState.IsValid)
            {
                var album = new Album();
                album.Nome = albumDTO.Nome;
                album.AnoLancamento = albumDTO.AnoLancamento;

                _database.Albuns.Add(album);
                _database.SaveChanges();

                var albunsDoBanco = _database.Albuns.ToList();
                
                var albumArtista = new AlbumArtista();

                foreach (var artistaId in albumDTO.ArtistaIds)
                {
                    albumArtista.AlbumId = albunsDoBanco.Last().Id;
                    albumArtista.ArtistaId = artistaId;

                    _database.AlbunsArtistas.Add(albumArtista);
                    _database.SaveChanges();
                }

                var albumGenero = new AlbumGenero();

                foreach (var generoId in albumDTO.GeneroIds)
                {
                    albumGenero.AlbumId = albunsDoBanco.Last().Id;
                    albumGenero.GeneroMusicalId = generoId;

                    _database.AlbunsGeneros.Add(albumGenero);
                    _database.SaveChanges();
                }

                var albumMusica = new AlbumMusica();

                foreach (var musicaId in albumDTO.MusicaIds)
                {
                    albumMusica.AlbumId = albunsDoBanco.Last().Id;
                    albumMusica.MusicaId = musicaId;

                    _database.AlbunsMusicas.Add(albumMusica);
                    _database.SaveChanges();
                }

                Response.StatusCode = 201;
                return new ObjectResult(new{info = "Novo album adicionado."});
            }
            
            Response.StatusCode = 401;
            return new ObjectResult(new{info = "Erro ao adicionar album."}); 
        }

        [HttpPatch]
        public IActionResult EditarAlbum([FromBody] Album album)
        {
            if (album.Id < 1)
            {
                Response.StatusCode = 400;

                return new ObjectResult(new{msg = "Id inválido."});
            }
            else
            {
                try
                {
                    var albumEditado = _database.Albuns.First(registro => registro.Id == album.Id);

                    albumEditado.Nome = album.Nome != null ? album.Nome : albumEditado.Nome;
                    albumEditado.AnoLancamento = album.AnoLancamento != 0 ? album.AnoLancamento : albumEditado.AnoLancamento;
                    
                    _database.SaveChanges();

                    return Ok();
                }
                catch (InvalidOperationException)
                {
                    Response.StatusCode = 400;

                    return new ObjectResult(new{msg = "Album não encontrado."});
                }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult ExcluirAlbum(int id)
        {
            try
            {
                var album = _database.Albuns.First(album => album.Id == id);

                _database.Albuns.Remove(album);
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