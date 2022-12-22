using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // using var db = new Data.ApplicationContext();

            // var existe = db.Database.GetPendingMigrations().Any();
            // if(existe)
            // {

            // }            
            // Console.WriteLine("Hello World!!!");

            // InserirDados();
            // InserirDadosEmMassa();
            //ConsultarDados();
            //CadastrarPedido();

            //ConsultaPedidoCarregamentoAdiantado();

            //AtualizarDados();
            RemoverRegistro();
        }


        private static void RemoverRegistro()
        {
            using var db = new Data.ApplicationContext();
            //var cliente = db.Clientes.Find(2);
            //db.Clientes.Remove(cliente);
            //db.Remove(cliente);
            //db.Entry(cliente).State = EntityState.Deleted;

            var cliente =  new Cliente
            {
                Id = 3
            };
            db.Entry(cliente).State = EntityState.Deleted;

            db.SaveChanges();
        }

        private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();
            //var cliente = db.Clientes.Find(1);
            var cliente = new Cliente
            {
                Id = 1
            };

            var clienteDesconectado = new 
            {
                Nome = "Cliente desconectado Passo 3",
                Telefone = "7966669999"
            };
            db.Attach(cliente);
            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);
            //cliente.Nome = "Clinte Alterado passo 2";

            //db.Clientes.Update(cliente);
            db.SaveChanges();
        }
        private static void ConsultaPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();

            var pedidos = db.Pedidos
                .Include(p => p.Itens)
                .ThenInclude(p => p.Produto)
                .ToList();

            Console.WriteLine(pedidos.Count);
        }

        private static void CadastrarPedido(){
            using var db = new Data.ApplicationContext();
            
            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10
                    }

                }
            };

            db.Pedidos.Add(pedido);
            db.SaveChanges();
        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();

            //var consultaPorSistaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();
            var consultaPorMetodo = db.Clientes
                .AsNoTracking()
                .Where(c => c.Id > 0)
                .OrderBy(p => p.Id)
                .ToList();
                
            foreach (var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consulta cliente: {cliente.Id}");
                // db.Clientes.Find(cliente.Id);
                db.Clientes.FirstOrDefault(c => c.Id == cliente.Id);
            }


        }
        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Carmosito Neto",
                CEP = "83412580",
                Cidade = "Curitiba",
                Estado = "PR",
                Telefone = "9900001111"
            };


            var listaClientes = new[]
            {
                new Cliente
                {
                    Nome = "Teste 1",
                    CEP = "83412580",
                    Cidade = "Curitiba",
                    Estado = "PR",
                    Telefone = "9900002222"
                },
                new Cliente
                {
                    Nome = "Teste 2",
                    CEP = "83412580",
                    Cidade = "Curitiba",
                    Estado = "PR",
                    Telefone = "9900003333"
                },
            };
            using var db = new Data.ApplicationContext();

            // db.AddRange(produto, cliente);
            db.Clientes.AddRange(listaClientes);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro: {registros}");
        }
        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new Data.ApplicationContext();

            db.Produtos.Add(produto);
            db.Set<Produto>().Add(produto);
            db.Entry(produto).State = EntityState.Added;
            db.Add(produto);

            var registros = db.SaveChanges();

            Console.WriteLine(registros);
        }
    }
}