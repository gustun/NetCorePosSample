using System.Linq;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pos.Api.Infrastructure;
using Pos.Api.ViewModel;
using Pos.BusinessLogic.Dto;
using Pos.BusinessLogic.Interface;

namespace Pos.Api.Controllers
{
    [Route("v1/campaigns")]
    [ApiController]
    public class CampaignController : BaseApiController
    {
        private readonly ICampaignManager _campaignManager;
        private readonly IMapper _mapper;

        public CampaignController(ICampaignManager campaignManager, IMapper mapper)
        {
            _campaignManager = campaignManager;
            _mapper = mapper;
        }
        
        [HttpGet]
        public IActionResult Get(int pageIndex = 1, int pageSize = 5)
        {
            var pagedResult = _campaignManager.GetWithPaging(pageIndex, pageSize);
            var vmList = pagedResult.Results.Select(_mapper.Map<CampaignViewModel>).ToList();
            return Result(pagedResult.CloneTo(vmList));
        }

        [HttpPost]
        public IActionResult Post([FromBody] NewCampaignViewModel vm)
        {
            var dto = _mapper.Map<CampaignDto>(vm);
            dto = _campaignManager.Add(dto);
            return Result(dto, dto.IsSuccess ? HttpStatusCode.Created : HttpStatusCode.BadRequest);
        }
    }
}
