using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Infra.Data.UoW;
using SolicitacaoCompraAgg = SistemaCompra.Domain.SolicitacaoCompraAggregate;

namespace SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra
{
    public class RegistrarCompraCommandHandler : CommandHandler, IRequestHandler<RegistrarCompraCommand, bool>
    {
        private readonly SolicitacaoCompraAgg.ISolicitacaoCompraRepository _solicitacaoCompraRepository;
        private readonly IProdutoRepository _produtoRepository;

        public RegistrarCompraCommandHandler(
            IUnitOfWork uow, 
            IMediator mediator,
            SolicitacaoCompraAgg.ISolicitacaoCompraRepository solicitacaoCompraRepository, 
            IProdutoRepository produtoRepository) : base(uow, mediator)
        {
            _solicitacaoCompraRepository = solicitacaoCompraRepository;
            _produtoRepository = produtoRepository;
        }

        public async Task<bool> Handle(RegistrarCompraCommand request, CancellationToken cancellationToken)
        {
            var solicitacaoCompra =
                new SolicitacaoCompraAgg.SolicitacaoCompra(request.UsuarioSolicitante, request.NomeFornecedor);

            foreach (var item in request.itens)
            {
                solicitacaoCompra.AdicionarItem(_produtoRepository.Obter(item.Produto.Id), item.Qtde);
            }
            
            solicitacaoCompra.RegistrarCompra(solicitacaoCompra.Itens);
            _solicitacaoCompraRepository.RegistrarCompra(solicitacaoCompra);

            Commit();
            PublishEvents(solicitacaoCompra.Events);

            return true;
        }
    }
}