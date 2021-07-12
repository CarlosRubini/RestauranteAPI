using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioClasses
{
	public class MateriaDAO : DAO<MateriaFiltro,Materia>
	{
		public MateriaDAO(IDbConnection connection) : base(connection)
		{
		}

		public override string GetSqlSelect(MateriaFiltro filtro)
		{
			Validate(filtro);

			var strSQL = $@" SELECT Mat.CodMateria
								   ,Mat.Titulo
								   ,Mat.Descricao
								   ,Mat.Tipo
								   ,Mat.Imagem
								   ,Mat.dataMateria
							 FROM Materia Mat
							 WHERE 1=1";
			if (filtro.CodMateria.HasValue)
			{
				strSQL += $" AND Mat.CodMateria = {filtro.CodMateria}";
			}
			if (filtro.Tipo != null)
			{
				strSQL += $" AND Mat.tipo = '{filtro.Tipo}'";
			}
			return strSQL;
		}

		public override List<Materia> FindAll(MateriaFiltro filtro)
		{
			Connection.Open();
			List<Materia> materias = new List<Materia>();
			IDbCommand dbCommand = Connection.CreateCommand();
			dbCommand.CommandText = GetSqlSelect(filtro);
			using (IDataReader dr = dbCommand.ExecuteReader())
			{
				while (dr.Read())
				{
					materias.Add(LoadObject(dr));
				}
			}
			Connection.Close();
			return materias;
		}

		public override void Delete(Materia obj)
		{
			Connection.Open();
			IDbCommand dbCommand = Connection.CreateCommand();
			dbCommand.CommandText = $"DELETE FROM Materia WHERE codMateria={obj.CodMateria}";

			var res = dbCommand.ExecuteNonQuery();
			Connection.Close();
			if (res != 1) throw new ApplicationException($"Esperado afetar uma linha. Linhas afetadas: {res}");
		}

		internal override Materia LoadObject(IDataReader dr)
		{
			return new Materia(true,
							   (int)dr["CodMateria"],
							   (string)dr["Titulo"],
							   (string)dr["Descricao"],
							   (string)dr["Tipo"],
							   (byte[])dr["imagem"],
							   (string)dr["dataMateria"]);
		}

		public override void Save(Materia obj)
		{
			Connection.Open();
			IDbCommand dbCommand = Connection.CreateCommand();

			var str = new StringBuilder();
			str.Append("0x");
			foreach (byte value in obj.Imagem)
			{
				str.AppendFormat("{0:X2}", value);
			}

			if (obj.Existe)
			{
				var strSQL = $@"UPDATE Materia SET descricao='{obj.Descricao}',tipo='{obj.Tipo}',titulo='{obj.Titulo}',imagem={str.ToString()}, dataMateria='{obj.DataMateria}' WHERE codMateria={obj.CodMateria}";
				dbCommand.CommandText = strSQL;
			}
			else
			{
				obj.CodMateria = GetProximoCodigo(obj);
				var strSQL = $@"INSERT INTO Materia (codMateria,titulo,descricao,tipo,imagem,dataMateria) VALUES ({obj.CodMateria},'{obj.Titulo}', '{obj.Descricao}', '{obj.Tipo}', {str.ToString()}, '{obj.DataMateria}')";
				dbCommand.CommandText = strSQL;
			}


			var res = dbCommand.ExecuteNonQuery();
			if (res != 1) throw new ApplicationException($"Esperado afetar 1 linha. Linhas afetadas: {res}.");
			obj.Existe = true;
			Connection.Close();
		}

		public int GetProximoCodigo(Materia obj)
		{
			IDbCommand dbCommand = Connection.CreateCommand();
			dbCommand.CommandText = "SELECT ISNULL(MAX(codMateria),0)+1 FROM Materia";
			var codigo = 0;
			using (IDataReader dr = dbCommand.ExecuteReader())
			{
				if (dr.Read())
				{
					codigo = (int)dr[0];
				}
			}
			return codigo;
		}
	}
}