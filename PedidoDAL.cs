using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loja.Models;
using System.Data.SqlClient;
using System.Data;

namespace Loja.DAL
{
    public class PedidoDAL : IPedidosDados
    {
        public void Alterar(Pedido pedido)
        {
            var pedidoOriginal = ObterPorId(pedido.Id);
            if (pedidoOriginal == null)
            {
                return;
            }
            var cn = new SqlConnection(DbHelper.conexao);

            var cmd1 = new SqlCommand("PedidoAlterar");
            cmd1.Connection = cn;
            cmd1.CommandType = CommandType.StoredProcedure;

            var cmd2 = new SqlCommand("PedidoItemAlterar");
            cmd2.Connection = cn;
            cmd2.CommandType = CommandType.StoredProcedure;

            var cmd3 = new SqlCommand("PedidoItemIncluir");
            cmd3.Connection = cn;
            cmd3.CommandType = CommandType.StoredProcedure;

            var cmd4 = new SqlCommand("PedidoItemExcluirExtras");
            cmd4.Connection = cn;
            cmd4.CommandType = CommandType.StoredProcedure;

            cn.Open();
            var tx = cn.BeginTransaction();
            try
            {
                cmd1.Transaction = tx;
                cmd2.Transaction = tx;
                cmd3.Transaction = tx;
                cmd4.Transaction = tx;


                //Pedido
                cmd1.Parameters.AddWithValue("@Id", pedido.Id);
                cmd1.Parameters.AddWithValue("@Data", pedido.Data);
                cmd1.Parameters.AddWithValue("@ClienteId", pedido.Cliente.Id);
                cmd1.Parameters.AddWithValue("@FormaPagamentoId", (int)pedido.FormaPagamento);

                int total = cmd1.ExecuteNonQuery();

                int ordem = 1;

                //Alterar
                cmd2.Parameters.AddWithValue("@PedidoId", pedido.Id);
                cmd2.Parameters.AddWithValue("@Ordem", 0);
                cmd2.Parameters.AddWithValue("@Quantidade", 0);
                cmd2.Parameters.AddWithValue("@Preco", Convert.ToDecimal(0));
                cmd2.Parameters.AddWithValue("@ProdutoId", string.Empty);

                //INcluir
                cmd3.Parameters.AddWithValue("@PedidoId", pedido.Id);
                cmd3.Parameters.AddWithValue("@Ordem", 0);
                cmd3.Parameters.AddWithValue("@Quantidade", 0);
                cmd3.Parameters.AddWithValue("@Preco", Convert.ToDecimal(0));
                cmd3.Parameters.AddWithValue("@ProdutoId", string.Empty);

                //Apagar Ultimos
                cmd4.Parameters.AddWithValue("@PedidoId", pedido.Id);
                cmd4.Parameters.AddWithValue("@Ordem", pedido.Itens.Count);

                foreach (var item in pedido.Itens)
                {
                    if (ordem <= pedidoOriginal.Itens.Count)
                    {
                        cmd2.Parameters["@ProdutoId"].Value = item.Produto.Id;
                        cmd2.Parameters["@Ordem"].Value = ordem;
                        cmd2.Parameters["@Quantidade"].Value = item.Quantidade;
                        cmd2.Parameters["@Preco"].Value = item.Preco;
                        cmd2.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd3.Parameters["@ProdutoId"].Value = item.Produto.Id;
                        cmd3.Parameters["@Ordem"].Value = ordem;
                        cmd3.Parameters["@Quantidade"].Value = item.Quantidade;
                        cmd3.Parameters["@Preco"].Value = item.Preco;
                        cmd3.ExecuteNonQuery();
                    }
                    ordem = ordem + 1;
                }

                //Apaga os restantes
                if (ordem < pedidoOriginal.Itens.Count)
                {
                    cmd4.Parameters["@Ordem"].Value = ordem;
                    cmd4.ExecuteNonQuery();
                }

                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                throw new Exception("Erro no servidor:" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        public void Excluir(int pedidoId)
        {
            DbHelper.ExecuteNonQuery("PedidoExcluir", "@Id", pedidoId);
        }

        public void Incluir(Pedido pedido)
        {
            var cn = new SqlConnection(DbHelper.conexao);
            var cmd1 = new SqlCommand("PedidoIncluir");
            cmd1.Connection = cn;
            cmd1.CommandType = CommandType.StoredProcedure;
            var cmd2 = new SqlCommand("PedidoItemIncluir");
            cmd2.Connection = cn;
            cmd2.CommandType = CommandType.StoredProcedure;
            cn.Open();
            var tx = cn.BeginTransaction();
            try
            {
                cmd1.Transaction = tx;
                cmd2.Transaction = tx;
                cmd1.Parameters.AddWithValue("@Data", pedido.Data);
                cmd1.Parameters.AddWithValue("@ClienteId", pedido.Cliente.Id);
                cmd1.Parameters.AddWithValue("FormaPagamentoId", pedido.FormaPagamento);
                pedido.Id = Convert.ToInt32(cmd1.ExecuteScalar());
                int ordem = 1;
                cmd2.Parameters.AddWithValue("@PedidoId", pedido.Id);
                cmd2.Parameters.AddWithValue("@Ordem", 0);
                cmd2.Parameters.AddWithValue("@Quantidade", 0);
                cmd2.Parameters.AddWithValue("@Preco", Convert.ToDecimal(0));
                cmd2.Parameters.AddWithValue("@ProdutoId", string.Empty);
                foreach (var item in pedido.Itens)
                {
                    cmd2.Parameters["@ProdutoId"].Value = item.Produto.Id;
                    cmd2.Parameters["@Ordem"].Value = ordem;
                    cmd2.Parameters["@Quantidade"].Value = item.Quantidade;
                    cmd2.Parameters["@Preco"].Value = item.Preco;
                    cmd2.ExecuteNonQuery();
                    ordem = ordem + 1; 
                }
                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                throw new Exception("Erro no servidor" + ex.Message);
            }
            finally
            {
                cn.Close();
            }

        }

        public Pedido ObterPorId(int id)
        {
            Pedido pedido = null;
            using (var reader = DbHelper.ExecuteReader("PedidoObterPorId", "@Id", id))
            {
                if (reader.Read())
                {
                    pedido = ObterPedido(reader);
                    pedido.Id = id;
                }
            }

            if (pedido != null)
            {
                pedido.Itens = new List<Pedido.Item>();
                using (var reader = DbHelper.ExecuteReader("PedidoObterPorPedidoId", "@Id", id))
                {
                    while (reader.Read())
                    {
                        pedido.Itens.Add(new Pedido.Item()
                        {
                            Ordem = (int)reader["Ordem"],
                            Preco = (decimal)reader["Preco"],
                            Produto = new Produto()
                            {
                                Id = reader["ProdutoId"].ToString(),
                                Nome = reader["ProdutoNome"].ToString()
                            },
                            Quantidade = (int)reader["Quantidade"]
                        });
                    }

                }
            }
            return pedido;
        }

        private Pedido ObterPedido(IDataReader reader)
        {
            return new Pedido()
            {
                Id = (int)reader["Id"],
                Data = (DateTime)reader["Data"],
                Cliente = ObterCliente(reader),
                FormaPagamento = (FormaPagamentoEnum)reader["FormaPagamentoId"]
            };
        }

        private Cliente ObterCliente(IDataReader reader)
        {
            return new Cliente()
            {
                Id = reader["ClienteId"].ToString(),
                Nome = reader["ClienteNome"].ToString()
            };
        }

        public List<Pedido> ObterTodos()
        {
            var lista = new List<Pedido>();
            using (var reader = DbHelper.ExecuteReader("PedidoListar"))
            {
                while (reader.Read())
                {
                    lista.Add(ObterPedido(reader));
                }
            }
            return lista;
        }
    }
}
