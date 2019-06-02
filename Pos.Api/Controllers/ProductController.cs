using System;
using System.Linq;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pos.Api.Infrastructure;
using Pos.Api.ViewModel;
using Pos.BusinessLogic.Dto;
using Pos.BusinessLogic.Dto.Base;
using Pos.BusinessLogic.Interface;

namespace Pos.Api.Controllers
{
    [Route("v1/products")]
    [ApiController]
    public class ProductController : BaseApiController
    {
        private readonly IProductManager _productManager;
        private readonly IMapper _mapper;

        public ProductController(IProductManager productManager, IMapper mapper)
        {
            _productManager = productManager;
            _mapper = mapper;
        }
        
        [HttpGet]
        public IActionResult Get(int pageIndex = 1, int pageSize = 5)
        {
            var pagedResult = _productManager.GetWithPaging(pageIndex, pageSize);
            var vmList = pagedResult.Results.Select(_mapper.Map<ProductViewModel>).ToList();
            return Result(pagedResult.CloneTo(vmList));
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var dto = _productManager.Get(id);
            if (!dto.IsSuccess) return Result(dto);
            return Result(_mapper.Map<ProductViewModel>(dto));
        }

        [HttpPost]
        public IActionResult Post([FromBody] NewProductModel vm)
        {
            var dto = _mapper.Map<ProductDto>(vm);
            dto = _productManager.Add(dto);
            return Result(dto, dto.IsSuccess ? HttpStatusCode.Created : HttpStatusCode.BadRequest);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] NewProductModel vm)
        {
            var dto = new ProductDto {Id = id};
            _mapper.Map(vm, dto);
            var result = _productManager.Update(dto);
            return Result(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = _productManager.Delete(id);
            return Result(result);
        }
    }
}