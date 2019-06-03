using System;
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
using Pos.Core.Interface;

namespace Pos.Api.Controllers
{
    [Route("v1/users")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly IUserManager _userManager;
        private readonly ICryptoHelper _cryptoHelper;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IMapper _mapper;

        public UserController(IUserManager userManager,
            ICryptoHelper cryptoHelper,
            IMapper mapper,
            IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _cryptoHelper = cryptoHelper;
            _mapper = mapper;
            _jwtOptions = jwtOptions.Value;
        }


        /// <summary>
        /// Generates an authentication token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous, HttpPost, Route("~/v1/login")]
        public IActionResult Login(LoginViewModel model)
        {
            var toReturn = new BaseResponse();
            var userDto = _userManager.GetUserByUserName(model.UserName);
            if (userDto == null)
                return Result(toReturn.AddError("User not found."), HttpStatusCode.NotFound);

            if (userDto.Password != _cryptoHelper.Hash(model.Password))
                return Result(toReturn.AddError("Password is not correct."));

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
            return Result(toReturn);
        }

        [AllowAnonymous, HttpPost]
        public IActionResult Post(NewUserViewModel viewModel)
        {
            var dto = _mapper.Map<UserDto>(viewModel);
            dto = _userManager.Add(dto);
            return Result(dto, dto.IsSuccess ? HttpStatusCode.Created : HttpStatusCode.BadRequest);
        }

        [HttpGet, Route("~/v1/me")]
        public IActionResult GetUserFromToken()
        {
            var userId = GetUserId();
            var dto = _userManager.Get(userId);
            if (!dto.IsSuccess) return Result(dto);
            return Result(_mapper.Map<UserViewModel>(dto));
        }
    }
}