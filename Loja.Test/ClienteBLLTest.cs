using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Loja.Models;
using Loja.BLL;
using Loja.DAL;

namespace Loja.Test
{
    public class ClienteDALMock : IClienteDados
    {
        public void Alterar(Cliente cliente)
        {

        }

        public void Excluir(string Id)
        {

        }

        public void Incluir(Cliente cliente)
        {

        }

        public Cliente ObterPorEmail(string email)
        {
            return null;
        }

        public Cliente ObterPorId(string id)
        {
            return null;
        }

        public List<Cliente> ObterTodos()
        {
            return null;
        }
    }
    [TestClass]
    public class ClienteBLLTest
    {
        [TestMethod]
        public void IncluirNomeNotNullTest()
        {
            var cliente = new Cliente()
            {
                Nome = "Jose da Silva",
                Email = "email@teste.com.br",
                Telefone = "12345678"
            };
            var dal = new ClienteDALMock();
            var bll = new ClienteBLL(dal);
            bool ok = false;
            try
            {
                bll.Incluir(cliente);
                ok = true;
            }
            catch (ApplicationException)
            {
                ok = false;
            }
            Assert.IsTrue(ok, "Deveria ter disparado um Exception");
        }
        [TestMethod]
        public void IncluirNomeNullTest()
        {
            var cliente = new Cliente()
            {
                Nome = null,
                Email = "email@teste.com.br",
                Telefone = "12345678"
            };
            var dal = new ClienteDAL();
            var bll = new ClienteBLL(dal);
            bool ok = false;
            try
            {
                bll.Incluir(cliente);
            }
            catch (ApplicationException)
            {
                ok = true;
            }
            Assert.IsTrue(ok, "Deveria ter disparado um Exception");
        }
    }
}
