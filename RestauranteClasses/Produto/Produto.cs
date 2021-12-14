using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestauranteClasses
{
	public class Produto
	{
		public bool Existe { get; set; }
		public int CodProduto { get; set; }
		public string NomeProduto { get; set; }
		public double PrecoProduto { get; set; }
		public string ImagemProduto { get; set; }
		public string SituacaoProduto { get; set; }
		public string DescricaoProduto { get; set; }
		public Produto(bool existe, int codProduto, string nomeProduto, double precoProduto, string imagemProduto,string situacaoProduto, string descricao)
		{
			Existe = existe;
			CodProduto = codProduto;
			NomeProduto = nomeProduto;
			PrecoProduto = precoProduto;
			ImagemProduto = imagemProduto;
			SituacaoProduto = situacaoProduto;
			DescricaoProduto = descricao;
		}
	}
}
