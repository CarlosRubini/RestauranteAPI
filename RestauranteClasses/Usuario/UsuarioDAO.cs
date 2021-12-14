using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestauranteClasses
{
	public class UsuarioDAO : DAO<UsuarioFiltro,Usuario>
	{
		public UsuarioDAO(IDbConnection connection) : base(connection)
		{
		}

		public override string GetSqlSelect(UsuarioFiltro filtro)
		{
			Validate(filtro);

			var strSQL = $@" SELECT US.codUsuario
								   ,US.nomeUsuario
								   ,US.senhaUsuario
								   ,US.emailUsuario
								   ,US.tipoUsuario
							 FROM Usuario US
							 WHERE 1=1";
			if (filtro.CodUsuario.HasValue)
			{
				strSQL += $" AND US.codUsuario = {filtro.CodUsuario}";
			}
			if(filtro.EmailUsuario != null && filtro.EmailUsuario != "")
			{
				strSQL += $" AND US.emailUsuario = '{filtro.EmailUsuario}'";
			}
			return strSQL;
		}

		public override List<Usuario> FindAll(UsuarioFiltro filtro)
		{
			Connection.Open();
			List<Usuario> materias = new List<Usuario>();
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

		public override void Delete(Usuario obj)
		{
			Connection.Open();
			IDbCommand dbCommand = Connection.CreateCommand();
			dbCommand.CommandText = $"DELETE FROM Usuario WHERE codUsuario={obj.CodUsuario}";

			var res = dbCommand.ExecuteNonQuery();
			Connection.Close();
			if (res != 1) throw new ApplicationException($"Esperado afetar uma linha. Linhas afetadas: {res}");
		}

		internal override Usuario LoadObject(IDataReader dr)
		{
			return new Usuario(true,
							   (int)dr["codUsuario"],
							   (string)dr["nomeUsuario"],
							   (string)dr["senhaUsuario"],
							   (string)dr["emailUsuario"],
							   (string)dr["tipoUsuario"]);
		}

		public override void Save(Usuario obj)
		{
			Connection.Open();
			IDbCommand dbCommand = Connection.CreateCommand();
			if (obj.Existe)
			{
				var strSQL = $@"UPDATE Usuario SET nomeUsuario='{obj.NomeUsuario}',senhaUsuario='{obj.SenhaUsuario}',emailUsuario='{obj.EmailUsuario}',tipoUsuario='{obj.TipoUsuario}' WHERE codUsuario={obj.CodUsuario}";
				dbCommand.CommandText = strSQL;
			}
			else
			{
				obj.CodUsuario = GetProximoCodigo();
				var strSQL = $@"INSERT INTO Usuario (codUsuario,nomeUsuario,senhaUsuario,emailUsuario,tipoUsuario) VALUES ({obj.CodUsuario},'{obj.NomeUsuario}', '{obj.SenhaUsuario}', '{obj.EmailUsuario}','{obj.TipoUsuario}')";
				dbCommand.CommandText = strSQL;
			}


			var res = dbCommand.ExecuteNonQuery();
			if (res != 1) throw new ApplicationException($"Esperado afetar 1 linha. Linhas afetadas: {res}.");
			obj.Existe = true;
			Connection.Close();
		}

		public int GetProximoCodigo()
		{
			IDbCommand dbCommand = Connection.CreateCommand();
			dbCommand.CommandText = "SELECT ISNULL(MAX(codUsuario),0)+1 FROM Usuario";
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