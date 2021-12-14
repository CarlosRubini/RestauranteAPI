using System;
using System.Collections.Generic;
using System.Text;

namespace RestauranteClasses
{
	public class Usuario
	{
		public bool Existe { get; set; }
		public int CodUsuario { get; set; }
		public string NomeUsuario { get; set; }
		public string SenhaUsuario { get; set; }
		public string EmailUsuario { get; set; }
		public string TipoUsuario { get; set; }
		public Usuario(bool existe, int codUsuario, string nomeUsuario, string senhaUsuario, string emailUsuario, string tipoUsuario)
		{
			Existe = existe;
			CodUsuario = codUsuario;
			NomeUsuario = nomeUsuario;
			SenhaUsuario = senhaUsuario;
			EmailUsuario = emailUsuario;
			TipoUsuario = tipoUsuario;
		}
	}
}
