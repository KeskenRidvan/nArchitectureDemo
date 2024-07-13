using Application.Features.Models.Queries.GetList;
using Core.Application.Requests;
using Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/models")]
[ApiController]
public class ModelsController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListModelQuery getListModelQuery = new()
        {
            PageRequest = pageRequest
        };

        GetListResponse<GetListModelResponseDto> response = await Mediator.Send(getListModelQuery);
        return Ok(response);
    }
}
