using Loja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loja.UI.Web.Models
{
    public class PedidoViewModel
    {
        public PedidoViewModel()
        {
            this.Clientes = new List<Cliente>();
            this.Produtos = new List<Produto>();
            this.Itens = new List<Item>();
            this.FormasPagamento = new List<string>();
            this.Data = DateTime.Now;
            this.Id = Id;
        }
        public int TotalQuantidade
        {
            get
            {
                return this.Itens.Sum(m => m.Quantidade);
            }
        }
        public decimal TotalPreco
        {
            get
            {
                return this.Itens.Sum(m => m.Total);
            }
        }
        public string NovoItemProdutoId { get; set; }
        public int NovoItemQuantidade { get; set; }
        public string ExcluirItemProdutoId { get; set; }
        public string Id { get; set; }
        public DateTime Data { get; set; }
        public List<Cliente> Clientes { get; set; }
        public string ClienteId { get; set; }
        public string ClienteNome { get; set; }
        public List<Item> Itens { get; set; }
        public FormaPagamentoEnum FormaPagamento { get; set; }
        public List<string> FormasPagamento { get; set; }
        public List<Produto> Produtos { get; set; }
        public class Item
        {
            public string ProdutoId { get; set; }
            public string ProdutoNome { get; set; }
            public int Quantidade { get; set; }
            public decimal Valor { get; set; }
            public decimal Total
            {
                get { return Valor * Quantidade; }
            }
        }

    }
}