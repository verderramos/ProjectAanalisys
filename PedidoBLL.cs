using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loja.Models;

namespace Loja.BLL
{
    public class PedidoBLL : IPedidosDados       
    {
        private IPedidosDados dal;
        public PedidoBLL(IPedidosDados pedidosDados)
        {
            this.dal = pedidosDados;
        }
        public void Incluir(Pedido pedido)
        {
            dal.Incluir(pedido);
        }
        public void Alterar(Pedido pedido)
        {
            dal.Alterar(pedido);
        }
        public void Excluir(int pedidoId)
        {
            dal.Excluir(pedidoId);
        }
        public Pedido ObterPorId(int id)
        {
            return dal.ObterPorId(id);
        }
        public List<Pedido> ObterTodos()
        {
            return dal.ObterTodos();
        }
    }
}
