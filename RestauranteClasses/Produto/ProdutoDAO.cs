using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestauranteClasses
{
	public class ProdutoDAO : DAO<ProdutoFiltro,Produto>
	{
		public ProdutoDAO(IDbConnection connection) : base(connection)
		{
		}

		public override string GetSqlSelect(ProdutoFiltro filtro)
		{
			Validate(filtro);

			var strSQL = $@" SELECT PD.codProduto
								   ,PD.nomeProduto
								   ,PD.precoProduto
								   ,PD.imagemProduto
								   ,PD.situacaoProduto
								   ,PD.descricaoProduto
							 FROM PRODUTO PD 
							 WHERE 1=1";
			if (filtro.CodProduto.HasValue)
			{
				strSQL += $" AND PD.codProduto = {filtro.CodProduto}";
			}
			return strSQL;
		}

		public override List<Produto> FindAll(ProdutoFiltro filtro)
		{
			Connection.Open();
			List<Produto> materias = new List<Produto>();
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

		public override void Delete(Produto obj)
		{
			Connection.Open();
			IDbCommand dbCommand = Connection.CreateCommand();
			dbCommand.CommandText = $"DELETE FROM Produto WHERE codProduto={obj.CodProduto}";

			var res = dbCommand.ExecuteNonQuery();
			Connection.Close();
			if (res != 1) throw new ApplicationException($"Esperado afetar uma linha. Linhas afetadas: {res}");
		}

		internal override Produto LoadObject(IDataReader dr)
		{
			return new Produto(true,
							   (int)dr["codProduto"],
							   (string)dr["nomeProduto"],
							   (double)dr["precoProduto"],
							   (string)dr["imagemProduto"],
							   (string)dr["situacaoProduto"],
							   (string)dr["descricaoProduto"]);
		}

		public override void Save(Produto obj)
		{
			Connection.Open();
			IDbCommand dbCommand = Connection.CreateCommand();
			if (obj.Existe)
			{
				var strSQL = $@"UPDATE Produto SET nomeProduto='{obj.NomeProduto}',precoProduto='{obj.PrecoProduto}',imagemProduto='{obj.ImagemProduto}',situacaoProduto='{obj.SituacaoProduto}',descricaoProduto='{obj.DescricaoProduto}' WHERE codProduto={obj.CodProduto}";
				dbCommand.CommandText = strSQL;
			}
			else
			{
				obj.CodProduto = GetProximoCodigo();
				var strSQL = $@"INSERT INTO Produto (codProduto,nomeProduto,precoProduto,imagemProduto,situacaoProduto,descricaoProduto) VALUES ({obj.CodProduto},'{obj.NomeProduto}', '{obj.PrecoProduto}', '{obj.ImagemProduto}','{obj.SituacaoProduto}','{obj.DescricaoProduto}')";
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
			dbCommand.CommandText = "SELECT ISNULL(MAX(codProduto),0)+1 FROM Produto";
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
