using System;
using System.Collections.Generic;
using System.Text;

namespace RadioClasses
{
	public class Materia
	{
		public bool Existe { get; set; }
		public int CodMateria { get; set; }
		public string Titulo { get; set; }
		public string Descricao { get; set; }
		public string Tipo { get; set; }
		public string DataMateria { get; set; }
		public byte[] Imagem { get; set; }
		

		public Materia(bool existe, int codMateria, string titulo, string descricao, string tipo, byte[] imagem, string data)
		{
			this.Existe = existe;
			this.CodMateria = codMateria;
			this.Titulo = titulo;
			this.Descricao = descricao;
			this.Tipo = tipo;
			this.Imagem = imagem;
			this.DataMateria = data;
		}
	}
}
