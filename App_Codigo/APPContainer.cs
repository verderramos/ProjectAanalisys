using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Loja.BLL;
using Loja.DAL;
using Loja.Models;

namespace Loja.UI.Web
{
    public class APPContainer
    {
        public static IClienteDados ObterClienteBLL()
        {
            var dal = new ClienteDAL();
            var bll = new ClienteBLL(dal);
            return bll;
        }
        public static IProdutosDados ObterProdutosbll()
        {
            var dal = new ProdutoDAL();
            var bll = new ProdutoBLL(dal);
            return bll;
        }
    }
}
