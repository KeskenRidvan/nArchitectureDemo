using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Brands.Queries.GetById;
public class GetByIdBrandQuery : IRequest<GetByIdBrandResponseDto>
{
    public Guid Id { get; set; }

    public class GetByIdBrandQueryHandler : IRequestHandler<GetByIdBrandQuery, GetByIdBrandResponseDto>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public GetByIdBrandQueryHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<GetByIdBrandResponseDto> Handle(GetByIdBrandQuery request, CancellationToken cancellationToken)
        {

            Brand? brand = await _brandRepository.GetAsync(
                 predicate: b => b.Id.Equals(request.Id),
                 withDeleted: false,
                 cancellationToken: cancellationToken);

            GetByIdBrandResponseDto response = _mapper.Map<GetByIdBrandResponseDto>(brand);
            return response;
        }
    }
}