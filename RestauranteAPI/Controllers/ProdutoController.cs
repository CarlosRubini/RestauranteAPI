using ApiRestaurante;
using RestauranteClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace RestauranteAPI
{
	[RoutePrefix("api/produto")]
	[Authorize]
	public class ProdutoController : ApiController
	{
		[Route("getProdutos")]
		[HttpGet]
		public IHttpActionResult GetAllProdutos()
		{
			List<Produto> produtos = new List<Produto>();
			using (IDbConnection connection = DatabaseHelper.CreateConnection())
			{
				ProdutoDAO produtoDAO = new ProdutoDAO(connection);
				ProdutoFiltro produtoFiltro = new ProdutoFiltro();
				produtos.AddRange(produtoDAO.FindAll(produtoFiltro));
			}
			return Ok(produtos);
		}

		[Route("criacao")]
		[HttpPost]
		public IHttpActionResult SalvarProduto(Produto produto)
		{
			if (produto == null) throw new ArgumentNullException(nameof(produto));

			using (IDbConnection connection = DatabaseHelper.CreateConnection())
			{
				ProdutoDAO produtoDAO = new ProdutoDAO(connection);
				produtoDAO.Save(produto);
			}
			return Ok(produto);
		}
	}
}