using MediatR;

namespace Application.Features.Brands.Commands.Create;
public class CreateBrandCommand : IRequest<CreatedBrandResponse>
{
    public string Name { get; set; }

    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, CreatedBrandResponse>
    {
        public Task<CreatedBrandResponse> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            CreatedBrandResponse response = new CreatedBrandResponse();
            response.Id = new Guid();
            response.Name = request.Name;
            response.CreatedDate = DateTime.Now;

            return null;
        }
    }
}
