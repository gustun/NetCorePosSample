﻿using System;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pos.Api.Infrastructure;
using Pos.Api.ViewModel;
using Pos.BusinessLogic.Dto;
using Pos.BusinessLogic.Interface;

namespace Pos.Api.Controllers
{
    [Route("v1/orders")]
    [ApiController]
    public class OrderController : BaseApiController
    {
        private readonly IOrderManager _orderManager;
        private readonly IMapper _mapper;

        public OrderController(IOrderManager orderManager, IMapper mapper)
        {
            _orderManager = orderManager;
            _mapper = mapper;
        }
        
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var dto = _orderManager.Get(id);
            if (!dto.IsSuccess) 
                return Result(dto);

            return Result(_mapper.Map<OrderViewModel>(dto));
        }

        [HttpPost]
        public IActionResult Post([FromBody] NewOrderViewModel order)
        {
            var newOrderDto = _mapper.Map<NewOrderDto>(order);
            var dto = _orderManager.Add(newOrderDto);
            if (!dto.IsSuccess)
                return Result(dto);

            return Result(_mapper.Map<OrderViewModel>(dto), HttpStatusCode.Created);
        }
    }
}