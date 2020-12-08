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
    public class ArtistasController : ControllerBase
    {
        private readonly ApplicationDbContext _database;
        private Hateoas _hateoas;

        public ArtistasController(ApplicationDbContext database)
        {
            _database = database;
            _hateoas = new Hateoas("localhost:5001/api/v1/Artistas");
            
            _hateoas.AddAction("GET_ARTISTA", "GET");
            _hateoas.AddAction("EDIT_ARTISTAS", "PATCH");
            _hateoas.AddAction("DELETE_ARTISTAS", "DELETE");
        }

        [HttpGet]
        public IActionResult GetArtista()
        {
            var listaDeArtistas = _database.Artistas.
            Include(x => x.AlbunsArtistas).ThenInclude(x => x.Album).
            ToList();

            var artistasHateoas = new List<Container<Artista>>();

            foreach (var artista in listaDeArtistas)
            {
                var artistaHateoas = new Container<Artista>();
                artistaHateoas.ObjetoContainer = artista;
                artistaHateoas.Links = _hateoas.GetActions(artista.Id.ToString());
                
                artistasHateoas.Add(artistaHateoas);
            }

            return Ok(artistasHateoas);
        }

        [HttpGet("{id}")]
        public IActionResult GetArtista(int id)
        {
            try
            {
                var artista = _database.Artistas.
                Include(x => x.AlbunsArtistas).ThenInclude(x => x.Album).
                First(artista => artista.Id == id);

                var artistaHateoas = new Container<Artista>();

                artistaHateoas.ObjetoContainer = artista;
                artistaHateoas.Links = _hateoas.GetActions(artista.Id.ToString());

                return Ok(artistaHateoas);
            }
            catch (InvalidOperationException)
            {
                Response.StatusCode = 404;
                return new ObjectResult(new{msg = "Nada encontrado."});
            }
        }

        [HttpPost]
        public IActionResult NovoArtista([FromBody] ArtistaDTO artistaDTO)
        {
            if (ModelState.IsValid)
            {
                var artista = new Artista();
                artista.Nome = artistaDTO.Nome;

                _database.Artistas.Add(artista);
                _database.SaveChanges();

                var artistasDoBanco = _database.Artistas.ToList();

                var albumArtista = new AlbumArtista();

                foreach (var albumId in artistaDTO.AlbumIds)
                {
                    albumArtista.ArtistaId = artistasDoBanco.Last().Id;
                    albumArtista.AlbumId = albumId;

                    _database.AlbunsArtistas.Add(albumArtista);
                    _database.SaveChanges();
                }
                
                Response.StatusCode = 201;
                return new ObjectResult(new{info = "Novo artista adicionado."});
            }

            Response.StatusCode = 401;
            return new ObjectResult(new{info = "Erro ao adicionar artista."});
        }

        [HttpPatch]
        public IActionResult EditarArtista([FromBody] Artista artista)
        {
            if (artista.Id < 1)
            {
                Response.StatusCode = 400;

                return new ObjectResult(new{msg = "Id inválido."});
            }
            else
            {
                try
                {
                    var artistaEditado = _database.Artistas.First(registro => registro.Id == artista.Id);
                    artistaEditado.Nome = artista.Nome != null ? artista.Nome : artistaEditado.Nome;

                    _database.SaveChanges();

                    return Ok();
                }
                catch (InvalidOperationException)
                {
                    Response.StatusCode = 400;

                    return new ObjectResult(new{msg = "artista não encontrado."});
                }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult ExcluirArtista(int id)
        {
            try
            {
                var artista = _database.Artistas.First(artista => artista.Id == id);

                _database.Artistas.Remove(artista);
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