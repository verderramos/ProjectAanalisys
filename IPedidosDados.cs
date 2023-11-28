using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Models
{
    public interface IPedidosDados
    {
        void Incluir(Pedido pedido);
        void Alterar(Pedido pedido);
        void Excluir(int pedidoI0d);
        List<Pedido> ObterTodos();
        Pedido ObterPorId(int id);
    }
}
