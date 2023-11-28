using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loja.BLL;
using Loja.DAL;
using Loja.Models;
using Loja.UI.Web.Models;

namespace Loja.UI.Web.Controllers
{
    public class PedidoController : Controller
    {
        private IPedidosDados bll;
        public PedidoController()
        {
            bll = APPContainer.ObterPedidosBll();
        }
        public ActionResult Detalhes(int id)
        {
            var pedido = bll.ObterPorId(id);
            PedidoViewModel pedidoViewModel = ObterViewModel(pedido);

            var bllProduto = APPContainer.ObterProdutosbll();
            var bllCliente = APPContainer.ObterClientesBLL();

            pedidoViewModel.Clientes = bllCliente.ObterTodos();

            pedidoViewModel.Produtos = bllProduto.ObterTodos();
            pedidoViewModel.Produtos.Insert(0, new Produto() { Id = string.Empty, Nome = string.Empty });

            pedidoViewModel.FormasPagamento = Enum.GetNames(typeof(FormaPagamentoEnum)).ToList();

            return View(pedidoViewModel);

        }

        private PedidoViewModel ObterViewModel(Pedido pedidoModel)
        {
            if (pedidoModel == null)
            {
                return new PedidoViewModel();
            }

            var pedidoViewModel = new PedidoViewModel();
            pedidoViewModel.Id = Convert.ToString(pedidoModel.Id);
            pedidoViewModel.Data = pedidoModel.Data;
            pedidoViewModel.ClienteId = pedidoModel.Cliente.Id;
            pedidoViewModel.ClienteNome = pedidoModel.Cliente.Nome;
            pedidoViewModel.FormaPagamento = pedidoModel.FormaPagamento;

            pedidoViewModel.Itens = new List<PedidoViewModel.Item>();
            if (pedidoModel.Itens != null)
            {
            
                int ordem = 1;
                foreach (var item in pedidoModel.Itens)
                {
                    pedidoViewModel.Itens.Add(new PedidoViewModel.Item()
                    {
                        ProdutoId = item.Produto.Id,
                        Valor = item.Preco,
                        Quantidade = item.Quantidade,
                        ProdutoNome = item.Produto.Nome
                    });
                    ordem++;
                }
            }
            return pedidoViewModel;

        }

        public ActionResult Alterar(int id)
        {
            var pedido = bll.ObterPorId(id);
            PedidoViewModel pedidoViewModel = ObterViewModel(pedido);
            var bllProduto = APPContainer.ObterProdutosbll();
            var bllCliente = APPContainer.ObterClientesBLL();
            pedidoViewModel.Clientes = bllCliente.ObterTodos();
            pedidoViewModel.Produtos = bllProduto.ObterTodos();
            pedidoViewModel.Produtos.Insert(0, new Produto() { Id = string.Empty, Nome = string.Empty });
            pedidoViewModel.FormasPagamento = Enum.GetNames(typeof(FormaPagamentoEnum)).ToList();
            return View(pedidoViewModel);
        }
        [HttpPost]
        public ActionResult Alterar(PedidoViewModel pedido)
        {
            var bllProduto = APPContainer.ObterProdutosbll();
            var bllCliente = APPContainer.ObterClientesBLL();
            pedido.Clientes = bllCliente.ObterTodos();
            pedido.Produtos = bllProduto.ObterTodos();
            pedido.Produtos.Insert(0, new Produto() { Id = string.Empty, Nome = string.Empty });
            pedido.FormasPagamento = Enum.GetNames(typeof(FormaPagamentoEnum)).ToList();
            if (Request.Form["incluirProduto"] == "Incluir")
            {
                ProcessarPedidoIncluir(pedido, bllProduto);
            }
            else if (Request.Form["excluirProduto"] == "Excluir")
            {
                ProcessarPedidoExcluir(pedido, bllProduto);
            }
            else if (Request.Form["Gravar"] == "Gravar")
            {

                var pedidoModel = ObterModel(pedido);
                bll.Alterar(pedidoModel);
                return RedirectToAction("Index");
            }
            return View(pedido);

        }

        private void ProcessarPedidoExcluir(PedidoViewModel pedido, IProdutosDados bllProduto)
        {
            var produto = bllProduto.ObterPorId(pedido.ExcluirItemProdutoId);
            if (produto != null)
            {
                var item = pedido.Itens.Where(m => m.ProdutoId == pedido.ExcluirItemProdutoId).FirstOrDefault();
                if (item != null)
                {
                    pedido.Itens.Remove(item);
                }
            }
        }

        private void ProcessarPedidoIncluir(PedidoViewModel pedido, IProdutosDados bllProduto)
        {
            var item = new PedidoViewModel.Item();
            item.ProdutoId = pedido.NovoItemProdutoId;
            item.Quantidade = pedido.NovoItemQuantidade;

            pedido.NovoItemProdutoId = string.Empty;
            pedido.NovoItemQuantidade = 0;

            var produto = bllProduto.ObterPorId(item.ProdutoId);
            if (produto != null)
            {

                item.Valor = produto.Preco;
                item.ProdutoNome = produto.Nome;

                var itemExistente = pedido.Itens.Where(m => m.ProdutoId == item.ProdutoId).FirstOrDefault();
                if (itemExistente == null)
                {
                    pedido.Itens.Add(item);
                }
                else
                {
                    itemExistente.Quantidade += item.Quantidade;
                }
            }
        }

        private Pedido ObterModel(PedidoViewModel pedidoViewModel)
        {
            var pedidoModel = new Pedido();
            pedidoModel.Id = Convert.ToInt32(pedidoViewModel.Id);
            pedidoModel.Data = pedidoViewModel.Data;
            pedidoModel.Cliente = new Cliente() { Id = pedidoViewModel.ClienteId };
            pedidoModel.FormaPagamento = pedidoViewModel.FormaPagamento;
            int ordem = 1;
            foreach (var item in pedidoViewModel.Itens)
            {
                pedidoModel.Itens.Add(new Pedido.Item()
                {
                    Ordem = ordem,
                    Preco = item.Valor,
                    Produto = new Produto() { Id = item.ProdutoId },
                    Quantidade = item.Quantidade
                });
                ordem++;
            }
            return pedidoModel;
        }

        public ActionResult Index()
        {
            var lista = bll.ObterTodos();
            var listaPedidosViewModel = new List<PedidoViewModel>();
            foreach (var pedido in lista)
            {
                listaPedidosViewModel.Add(ObterViewModel(pedido));
            }
            return View(listaPedidosViewModel);
        }
        public ActionResult Incluir()
        {
            var bllCliente = APPContainer.ObterClientesBLL();
            var bllProduto = APPContainer.ObterProdutosbll();
            var pedido = new PedidoViewModel();
            pedido.Clientes = bllCliente.ObterTodos();
            pedido.Produtos = bllProduto.ObterTodos();
            pedido.Produtos.Insert(0, new Produto() { Id = string.Empty, Nome = string.Empty });
            pedido.NovoItemProdutoId = string.Empty;
            pedido.NovoItemQuantidade = 0;
            pedido.FormasPagamento = Enum.GetNames(typeof(FormaPagamentoEnum)).ToList();

            return View(pedido);
        }
        [HttpPost]
        public ActionResult Incluir(PedidoViewModel pedido)
        {
            try
            {
                var bllCliente = APPContainer.ObterClientesBLL();
                var bllProduto = APPContainer.ObterProdutosbll();
                pedido.Clientes = bllCliente.ObterTodos();
                pedido.Produtos = bllProduto.ObterTodos();
                pedido.Produtos.Insert(0, new Produto() { Id = string.Empty, Nome = string.Empty });
                pedido.FormasPagamento = Enum.GetNames(typeof(FormaPagamentoEnum)).ToList();

                if (Request.Form["incluirProduto"] == "Incluir")
                {
                    ProcessarPedidoIncluir(pedido, bllProduto);
                }

                else if (Request.Form["excluirProduto"] == "Excluir")
                {
                    ProcessarPedidoExcluir(pedido, bllProduto);
                }

                else if (Request.Form["Gravar"] == "Gravar")
                {
                    var bll = APPContainer.ObterPedidosBll();
                    var pedidoModel = ObterModel(pedido);
                    bll.Incluir(pedidoModel);
                    var listaPedidosViewModel = bll.ObterTodos().Select(ObterViewModel).ToList();
                    return View("Index", listaPedidosViewModel);
                }
                return View(pedido);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro durante a inclusão do pedido: {ex.Message}";
                return View("Error");
            }
        }
    }
}