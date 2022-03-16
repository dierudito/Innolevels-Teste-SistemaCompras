using MediatR;
using SistemaCompra.Domain.SolicitacaoCompraAggregate;

namespace SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra
{
    public class RegistrarCompraCommand : IRequest<bool>
    {
        public string UsuarioSolicitante { get; set; }
        public string NomeFornecedor { get; set; }
        public Item[] itens { get; set; }
    }
}