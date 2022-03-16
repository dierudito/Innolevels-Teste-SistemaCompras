using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.Core.Model;
using SistemaCompra.Domain.ProdutoAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using SistemaCompra.Domain.SolicitacaoCompraAggregate.Events;

namespace SistemaCompra.Domain.SolicitacaoCompraAggregate
{
    public class SolicitacaoCompra : Entity
    {
        private const decimal Condicao_Pagamento_30_Dias = 50_000;
        public UsuarioSolicitante UsuarioSolicitante { get; private set; }
        public NomeFornecedor NomeFornecedor { get; private set; }
        public IList<Item> Itens { get; private set; }
        public DateTime Data { get; private set; }
        public Money TotalGeral { get; private set; }
        public Situacao Situacao { get; private set; }
        public CondicaoPagamento CondicaoPagamento { get; private set; }

        private SolicitacaoCompra()
        {
            Itens = new List<Item>(); }

        public SolicitacaoCompra(string usuarioSolicitante, string nomeFornecedor):this()
        {
            Id = Guid.NewGuid();
            UsuarioSolicitante = new UsuarioSolicitante(usuarioSolicitante);
            NomeFornecedor = new NomeFornecedor(nomeFornecedor);
            Data = DateTime.Now;
            Situacao = Situacao.Solicitado;
        }

        public void AdicionarItem(Produto produto, int qtde)
        {
            Itens.Add(new Item(produto, qtde));
        }

        public void RegistrarCompra(IEnumerable<Item> itens)
        {
           if(!itens.Any()) throw new BusinessRuleException("A solicitação de compra deve possuir itens!");
           var valor = itens.Sum(x => x.Subtotal.Value);
           TotalGeral = new Money(valor);
           if (valor >= Condicao_Pagamento_30_Dias) CondicaoPagamento = new CondicaoPagamento(30);

           AddEvent(new CompraRegistradaEvent(Id, itens, TotalGeral.Value));
        }
    }
}
