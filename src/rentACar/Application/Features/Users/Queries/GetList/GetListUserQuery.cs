using Application.Features.Users.Constants;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorizations;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Queries.GetList;
public class GetListUserQuery : IRequest<GetListResponse<GetListUserListResponseDto>>, ISecuredRequest
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [UsersOperationClaims.Read];

    public GetListUserQuery()
    {
        PageRequest = new PageRequest
        {
            PageIndex = 0,
            PageSize = 10
        };
    }

    public GetListUserQuery(PageRequest pageRequest)
    {
        PageRequest = pageRequest;
    }

    public class GetListUserQueryHandler : IRequestHandler<GetListUserQuery, GetListResponse<GetListUserListResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetListUserQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListUserListResponseDto>> Handle(
            GetListUserQuery request,
            CancellationToken cancellationToken)
        {
            Paginate<User> users = await _userRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                enableTracking: false,
                cancellationToken: cancellationToken);

            GetListResponse<GetListUserListResponseDto> response =
                _mapper.Map<GetListResponse<GetListUserListResponseDto>>(users);

            return response;
        }
    }
}