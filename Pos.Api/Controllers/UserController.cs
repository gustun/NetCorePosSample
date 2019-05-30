using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pos.Api.Infrastructure;
using Pos.Api.Options;
using Pos.Api.ViewModel;
using Pos.Api.ViewModel.Base;
using Pos.BusinessLogic.Dto;
using Pos.BusinessLogic.Dto.Base;
using Pos.BusinessLogic.Interface;
using Pos.Contracts;

namespace Pos.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly IUserManager _userManager;
        private readonly ICryptoHelper _cryptoHelper;
        private readonly ILoggerManager _loggerManager;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IMapper _mapper;

        public UserController(ILoggerManager logger, IUserManager userManager, ICryptoHelper cryptoHelper, ILoggerManager loggerManager, IMapper mapper, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _cryptoHelper = cryptoHelper;
            _loggerManager = loggerManager;
            _mapper = mapper;
            _jwtOptions = jwtOptions.Value;
        }

        [AllowAnonymous, HttpPost, Route("login")]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var toReturn = new BaseResponse();
            var userDto = _userManager.GetUserByUserName(model.UserName);
            if (userDto == null)
            {
                toReturn.AddError("User not found.");
                return NotFound(toReturn);
            }

            if (userDto.Password != _cryptoHelper.Hash(model.Password))
            {
                toReturn.AddError("Password is not correct.");
                return BadRequest(toReturn);
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Expires = _jwtOptions.Expiration,
                NotBefore = _jwtOptions.NotBefore,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, userDto.UserName),
                    new Claim(ClaimTypes.Sid,userDto.Id.ToString())
                }),
                SigningCredentials = _jwtOptions.SigningCredentials
            });

            toReturn.Data = new { UserToken = handler.WriteToken(token) };
            return Ok(toReturn);
        }

        [HttpGet, Route("{id}")]
        public IActionResult Get(Guid id)
        {
            var userId = GetUserId();
            return Ok(userId);
        }

        [AllowAnonymous, HttpPost]
        public IActionResult Post(UserCreateModel model)
        {
            var dto = _mapper.Map<UserDto>(model);
            dto = _userManager.Add(dto);
            return Result(dto, HttpStatusCode.Created);
        }
    }
}