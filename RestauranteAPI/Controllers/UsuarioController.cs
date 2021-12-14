using RestauranteClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiRestaurante
{
	[RoutePrefix("api/usuario")]
	[Authorize]
	public class UsuarioController : ApiController
	{
		[Route("getUsuarios")]
		[HttpGet]
		public IHttpActionResult GetAllUsuarios()
		{
			List<Usuario> usuarios = new List<Usuario>();
			using (IDbConnection connection = DatabaseHelper.CreateConnection())
			{
				UsuarioDAO usuarioDAO = new UsuarioDAO(connection);
				UsuarioFiltro usuarioFiltro = new UsuarioFiltro();
				usuarios.AddRange(usuarioDAO.FindAll(usuarioFiltro));
			}
			return Ok(usuarios);
		}

		[Route("criacao")]
		[HttpPost]
		public IHttpActionResult SalvarUsuario(Usuario usuario)
		{
			if (usuario == null) throw new ArgumentNullException(nameof(usuario));

			using (IDbConnection connection = DatabaseHelper.CreateConnection())
			{
				UsuarioDAO usuarioDAO = new UsuarioDAO(connection);

				//Valida se é necessário atualizar a senha.
				if (usuario.Existe)
				{
					UsuarioFiltro usuarioFiltro = new UsuarioFiltro { CodUsuario = usuario.CodUsuario };
					Usuario usuarioFind = usuarioDAO.FindOne(usuarioFiltro);
					if(usuarioFind != null && usuarioFind.SenhaUsuario != usuario.SenhaUsuario)
					{
						usuario.SenhaUsuario = Crypto.EncryptMD5(usuario.SenhaUsuario);
					}
				}
				else
				{
					usuario.SenhaUsuario = Crypto.EncryptMD5(usuario.SenhaUsuario);
				}
				usuarioDAO.Save(usuario);
			}
			return Ok(usuario);
		}

		[Route("loginUsuario")]
		[HttpGet]
		public IHttpActionResult LoginUsuario(string email,string password)
		{
			Usuario usuario = null;
			using (IDbConnection connection = DatabaseHelper.CreateConnection())
			{
				UsuarioDAO usuarioDAO = new UsuarioDAO(connection);
				UsuarioFiltro usuarioFiltro = new UsuarioFiltro {EmailUsuario = email};
				usuario = usuarioDAO.FindOne(usuarioFiltro);
				if (usuario != null && Crypto.Decrypt(usuario.SenhaUsuario) == Crypto.GerarHashMd5(password)) return Ok(usuario);
			}
			return Ok("");
		}
	}
}
