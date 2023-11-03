using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Models
{
    public interface IProdutosDados
    {
        void Incluir(Produto produto);
        void Alterar(Produto produto);
        void Excluir(string Id);
        List<Produto> ObterTodos();
        Produto ObterPorId(string id);
        
    }
}
