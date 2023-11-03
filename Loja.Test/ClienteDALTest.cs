using System;
using Loja.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Loja.Test
{
    [TestClass]
    public class ClienteDalTest
    {
        [TestMethod]
        public void ObterPorEmailNullTest()
        {
            string email = null;
            var dal = new ClienteDAL();
            bool ok = false;
            try
            {
                var cliente = dal.ObterPorEmail(email);
            }
            catch (ApplicationException ex)
            {
                if (ex.Message == "O email deve ser informado." + ex.Message)
                {
                    ok = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro no servidor. Parametro não informado." + ex.Message);
            }
            Assert.IsTrue(ok, "Deveria ter disparado o ApplicationException com a mensagem: 'O email deve ser informado.'");
        }

        [TestMethod]
        public void ObterPorEmailTest()
        {
            string email = "verderramos@gmail.com";
            var dal = new ClienteDAL();
            var cliente = dal.ObterPorEmail(email);
            if (cliente != null)
            {
                Console.WriteLine("Dados localizados:");
                Console.WriteLine(cliente.Id);
                Console.WriteLine(cliente.Nome);
                Console.WriteLine(cliente.Email);
                Console.WriteLine(cliente.Telefone);
            }
            Assert.IsTrue(cliente != null, "Deveria ter retornado uma estancia de um cliente");
        }
    }
}
