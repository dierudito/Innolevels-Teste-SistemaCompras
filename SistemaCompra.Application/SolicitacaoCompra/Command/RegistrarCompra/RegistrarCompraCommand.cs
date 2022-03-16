using ProdutoAgg = SistemaCompra.Domain.ProdutoAggregate;
using System.Collections.Generic;
using MediatR;

namespace SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra
{
    public class RegistrarCompraCommand : IRequest<bool>
    {
        public string UsuarioSolicitante { get; set; }
        public string NomeFornecedor { get; set; }
        public ProdutoAgg.Produto Produto { get; set; }
        public int Quantidade { get; set; }
    }
}