using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SistemaCompra.Infra.Data.UoW;

namespace SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra
{
    public class RegistrarCompraCommandHandler : CommandHandler, IRequestHandler<RegistrarCompraCommand, bool>
    {
        private readonly Domain.SolicitacaoCompraAggregate.ISolicitacaoCompraRepository _solicitacaoCompraRepository;

        public RegistrarCompraCommandHandler(
            IUnitOfWork uow, 
            IMediator mediator, 
            Domain.SolicitacaoCompraAggregate.ISolicitacaoCompraRepository solicitacaoCompraRepository) : base(uow, mediator)
        {
            _solicitacaoCompraRepository = solicitacaoCompraRepository;
        }

        public async Task<bool> Handle(RegistrarCompraCommand request, CancellationToken cancellationToken)
        {
            var solicitacaoCompra =
                new Domain.SolicitacaoCompraAggregate.SolicitacaoCompra(request.UsuarioSolicitante, request.NomeFornecedor);

            solicitacaoCompra.AdicionarItem(request.Produto, request.Quantidade);
            solicitacaoCompra.RegistrarCompra(solicitacaoCompra.Itens);
            _solicitacaoCompraRepository.RegistrarCompra(solicitacaoCompra);

            Commit();
            PublishEvents(solicitacaoCompra.Events);

            return true;
        }
    }
}