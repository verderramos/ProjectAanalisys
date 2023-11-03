using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Loja.DAL;
using Loja.Models;

namespace Loja.Test
{
    [TestClass]
    public class ProdutoDALTest
    {
        [TestMethod]
        public void Incluir()
        {
            var p = new Produto();
            p.Id = Guid.NewGuid().ToString();
            p.Nome = "Produto Teste";
            p.Preco = 100;
            p.Estoque = 2;

            var dal = new ProdutoDAL();
            dal.Incluir(p);

            var produto = dal.ObterPorId(p.Id);
            Assert.IsTrue(produto != null, "Erro na inclusão");
        }

        [TestMethod]
        public void Listar()
        {
            var dal = new ProdutoDAL();
            var lista = dal.ObterTodos();
            foreach (var p in lista)
            {
                Console.WriteLine(p.Id);
                Console.WriteLine(p.Nome);
                Console.WriteLine(p.Preco);
                Console.WriteLine(p.Estoque);
            }
            Assert.IsTrue(lista.Count > 0, "A lista não pode ser vazia.");
        }

    }
}
 