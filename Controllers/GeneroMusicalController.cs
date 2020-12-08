using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MusicPlayer.Data;
using MusicPlayer.DTO;
using MusicPlayer.Models;
using MusicPlayer.RESTful;

namespace MusicPlayer.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GeneroMusicalController : ControllerBase
    {
        private readonly ApplicationDbContext _database;
        private Hateoas _hateoas;

        public GeneroMusicalController(ApplicationDbContext database)
        {
            _database = database;
            _hateoas = new Hateoas("localhost:5001/api/v1/GeneroMusical");
            
            _hateoas.AddAction("GET_GENEROMUSICAL", "GET");
            _hateoas.AddAction("EDIT_GENEROMUSICAL", "PATCH");
            _hateoas.AddAction("DELETE_GENEROMUSICAL", "DELETE");
        }

        [HttpGet]
        public IActionResult GetGeneroMusical()
        {
            var listaDeGeneros = _database.GenerosMusicais.ToList();

            var generosMusicaisHateoas = new List<Container<GeneroMusical>>();

            foreach (var generoMusical in listaDeGeneros)
            {
                var generoMusicalHateoas = new Container<GeneroMusical>();
                generoMusicalHateoas.ObjetoContainer = generoMusical;
                generoMusicalHateoas.Links = _hateoas.GetActions(generoMusical.Id.ToString());
                
                generosMusicaisHateoas.Add(generoMusicalHateoas);
            }

            return Ok(generosMusicaisHateoas);
        }

        [HttpGet("{id}")]
        public IActionResult GetGeneroMusical(int id)
        {
            try
            {
                var genero = _database.GenerosMusicais.First(genero => genero.Id == id);

                var generoHateoas = new Container<GeneroMusical>();

                generoHateoas.ObjetoContainer = genero;
                generoHateoas.Links = _hateoas.GetActions(genero.Id.ToString());

                return Ok(generoHateoas);
            }
            catch (InvalidOperationException)
            {
                Response.StatusCode = 404;
                return new ObjectResult(new{msg = "Nada encontrado."});
            }
        }

        [HttpPost]
        public IActionResult NovoGeneroMusical([FromBody] GeneroMusicalDTO generoMusicalDTO)
        {
            var generoMusical = new GeneroMusical();
            generoMusical.Nome = generoMusicalDTO.Nome;

            _database.GenerosMusicais.Add(generoMusical);
            _database.SaveChanges();

            Response.StatusCode = 201;
            return new ObjectResult(new{info = "Novo genero musical adicionado."});
        }

        [HttpPatch]
        public IActionResult EditarGeneroMusical([FromBody] GeneroMusical generoMusical)
        {
            if (generoMusical.Id < 1)
            {
                Response.StatusCode = 400;

                return new ObjectResult(new{msg = "Id inválido."});
            }
            else
            {
                try
                {
                    var generoMusicalEditado = _database.GenerosMusicais.First(registro => registro.Id == generoMusical.Id);

                    generoMusicalEditado.Nome = generoMusical.Nome != null ? generoMusical.Nome : generoMusicalEditado.Nome;

                    _database.SaveChanges();

                    return Ok();
                }
                catch (InvalidOperationException)
                {
                    Response.StatusCode = 400;

                    return new ObjectResult(new{msg = "Genero musical não encontrado."});
                }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult ExcluirGeneroMusical(int id)
        {
            try
            {
                var generoMusical = _database.GenerosMusicais.First(generoMusical => generoMusical.Id == id);

                _database.GenerosMusicais.Remove(generoMusical);
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