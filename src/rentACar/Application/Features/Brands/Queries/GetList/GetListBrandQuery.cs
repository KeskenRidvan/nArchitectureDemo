using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Cachings;
using Core.Application.Pipelines.Loggings;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;

namespace Application.Features.Brands.Queries.GetList;
public class GetListBrandQuery : IRequest<GetListResponse<GetListBrandResponseDto>>, ICachableRequest, ILoggableRequest
{
    public PageRequest PageRequest { get; set; }

    public string CacheKey => $"GetListBrandQuery({PageRequest.PageIndex},{PageRequest.PageSize})";
    public bool BypassCache { get; }
    public TimeSpan? SlidingExpiration { get; }
    public string? CacheGroupKey => "GetBrands";

    public class GetListBrandQueryHandler :
        IRequestHandler<GetListBrandQuery, GetListResponse<GetListBrandResponseDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public GetListBrandQueryHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListBrandResponseDto>> Handle(
            GetListBrandQuery request,
            CancellationToken cancellationToken)
        {
            Paginate<Brand> brands = await _brandRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                withDeleted: false,
                cancellationToken: cancellationToken);

            GetListResponse<GetListBrandResponseDto> response =
                _mapper.Map<GetListResponse<GetListBrandResponseDto>>(brands);

            return response;
        }
    }
}
