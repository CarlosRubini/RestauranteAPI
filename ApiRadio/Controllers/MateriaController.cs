using RadioClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiRadio
{
	[RoutePrefix("api/materia")]
	[Authorize]
	public class MateriaController : ApiController
	{
		[Route("getmaterias")]
		[HttpGet]
		public async Task<IHttpActionResult> FindMaterias()
		{
			List<Materia> materias = new List<Materia>();
			using (IDbConnection connection = DatabaseHelper.CreateConnection())
			{
				MateriaDAO materiaDAO = new MateriaDAO(connection);
				MateriaFiltro materiaFiltro = new MateriaFiltro();
				materias.AddRange(materiaDAO.FindAll(materiaFiltro));
			}
			return Ok(materias);
		}
	}
}
