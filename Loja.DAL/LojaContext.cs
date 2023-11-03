using Loja.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.DAL
{
    public class LojaContext:DbContext
    {
        public LojaContext():base(DbHelper.conexao)
        {

        }
        public DbSet<Produto> Produtos { get; set; }
    }
}
