using System;
using System.Collections.Generic;
using System.Text;
using Loja.Models;

namespace Loja.DAL
{
    public class ClienteDAL : IClienteDados
    {
        public void Alterar(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        public void Excluir(string Id)
        {
            throw new NotImplementedException();
        }

        public void Incluir(Cliente cliente)
        {
            DbHelper.ExecuteNonQuery("ClienteIncluir",
                "@Id",cliente.Id,
                "@Nome",cliente.Nome,
                "@Email",cliente.Email,
                "@Telefone",cliente.Telefone,
                "@Endereco",cliente.Endereco
                );
        }

        public Cliente ObterPorEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Cliente ObterPorId(string id)
        {
            throw new NotImplementedException();
        }

        public List<Cliente> ObterTodos()
        {
            var lista = new List<Cliente>();
            using (var reader = DbHelper.ExecuteReader("ClienteListar"))
            {
                while (reader.Read())
                {
                    var cliente = new Cliente();
                    cliente.Id = reader["Id"].ToString();
                    cliente.Nome = reader["Nome"].ToString();
                    cliente.Email = reader["Email"].ToString();
                    cliente.Telefone = reader["Telefone"].ToString();
                    cliente.Endereco = reader["Endereco"].ToString();
                    lista.Add(cliente);
                }
            }
            return lista;
        }
    }
}
