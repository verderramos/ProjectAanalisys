using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loja.Models;


namespace Loja.UI.Web.Controllers
{
    public class ProdutoController : Controller
    {
        private IProdutosDados bll;
        public ProdutoController()
        {
            bll = APPContainer.ObterProdutosbll();
        }
        public ActionResult Excluir(string id)
        {
            var produto = bll.ObterPorId(id);
            return View(produto);
        }

        [HttpPost]
        public ActionResult Excluir(string id, FormCollection form)
        {
            try
            {
                bll.Excluir(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var produto = bll.ObterPorId(id);
                return View(produto);
            }
        }
        public ActionResult Alterar(string id)
        {
            var produto = bll.ObterPorId(id);
            return View(produto);
        }

        [HttpPost]
        public ActionResult Alterar(Produto produto)
        {
            try
            {
                bll.Alterar(produto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(produto);
            }
        }
        public ActionResult Detalhes(string id)
        {
            var produto = bll.ObterPorId(id);
            return View(produto);
        }
        public ActionResult Incluir()
        {
            var produto = new Produto();
            return View(produto);
        }
        [HttpPost]
        public ActionResult Incluir(Produto produto)
        {
            try
            {
                bll.Incluir(produto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(produto);
            }
        }
        public ActionResult Index()
        {
            var lista = bll.ObterTodos();
            return View(lista);
        }
    }
}