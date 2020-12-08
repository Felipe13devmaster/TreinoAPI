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
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _database;
        private Hateoas _hateoas;

        public UsuariosController(ApplicationDbContext database)
        {
            _database = database;
            _hateoas = new Hateoas("localhost:5001/api/v1/Usuarios");
            
            _hateoas.AddAction("GET_USUARIO", "GET");
            _hateoas.AddAction("EDIT_USUARIOS", "PATCH");
            _hateoas.AddAction("DELETE_USUARIOS", "DELETE");
        }

        [HttpGet]
        public IActionResult GetUsuario()
        {
            var listaDeUsuarios = _database.Usuarios.
            Include(x => x.UsuariosPlaylists).ThenInclude(x => x.Playlist).
            ToList();

            var usuariosHateoas = new List<Container<Usuario>>();

            foreach (var usuario in listaDeUsuarios)
            {
                var usuarioHateoas = new Container<Usuario>();
                usuarioHateoas.ObjetoContainer = usuario;
                usuarioHateoas.Links = _hateoas.GetActions(usuario.Id.ToString());
                
                usuariosHateoas.Add(usuarioHateoas);
            }

            return Ok(usuariosHateoas);
        }

        [HttpGet("{id}")]
        public IActionResult GetUsuario(int id)
        {
            try
            {
                var usuario = _database.Usuarios.
                Include(x => x.UsuariosPlaylists).ThenInclude(x => x.Playlist).
                First(usuario => usuario.Id == id);

                var usuarioHateoas = new Container<Usuario>();

                usuarioHateoas.ObjetoContainer = usuario;
                usuarioHateoas.Links = _hateoas.GetActions(usuario.Id.ToString());

                return Ok(usuarioHateoas);
            }
            catch (InvalidOperationException)
            {
                Response.StatusCode = 404;
                return new ObjectResult(new{msg = "Nada encontrado."});
            }
        }

        [HttpPost]
        public IActionResult NovoUsuario([FromBody] UsuarioDTO usuarioDTO)
        {
            if (ModelState.IsValid)
            {
                var usuario = new Usuario();
                usuario.Nome = usuarioDTO.Nome;
                usuario.Idade = usuarioDTO.Idade;

                _database.Usuarios.Add(usuario);
                _database.SaveChanges();

                var usuariosDoBanco = _database.Usuarios.ToList();

                var usuarioPlaylist = new UsuarioPlaylist();

                foreach (var playlistId in usuarioDTO.PlaylistIds)
                {
                    usuarioPlaylist.Usuarioid = usuariosDoBanco.Last().Id;
                    usuarioPlaylist.PlaylistId = playlistId;

                    _database.UsuariosPlaylists.Add(usuarioPlaylist);
                    _database.SaveChanges();
                }

                Response.StatusCode = 201;
                return new ObjectResult(new{info = "Novo usuário adicionado."});
            }

            Response.StatusCode = 401;
            return new ObjectResult(new{info = "Erro ao adicionar usuário."});
        }

        [HttpPatch]
        public IActionResult EditarUsuario([FromBody] Usuario usuario)
        {
            if (usuario.Id < 1)
            {
                Response.StatusCode = 400;

                return new ObjectResult(new{msg = "Id inválido."});
            }
            else
            {
                try
                {
                    var usuarioEditado = _database.Usuarios.First(registro => registro.Id == usuario.Id);

                    usuarioEditado.Nome = usuario.Nome != null ? usuario.Nome : usuarioEditado.Nome;
                    usuarioEditado.Idade = usuario.Idade != 0 ? usuario.Idade : usuarioEditado.Idade;

                    _database.SaveChanges();

                    return Ok();
                }
                catch (InvalidOperationException)
                {
                    Response.StatusCode = 400;

                    return new ObjectResult(new{msg = "usuario não encontrado."});
                }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult ExcluirUsuario(int id)
        {
            try
            {
                var usuario = _database.Usuarios.First(usuario => usuario.Id == id);

                _database.Usuarios.Remove(usuario);
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