using Pos.BusinessLogic.Dto;
using Pos.BusinessLogic.Dto.Base;
using Pos.BusinessLogic.Interface;
using Pos.DataAccess.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pos.Contracts;
using Pos.DataAccess;

namespace Pos.BusinessLogic
{
    public class UserManager : IUserManager
    {
        private readonly PosDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICryptoHelper _cryptoHelper;

        public UserManager(PosDbContext posDbContext, IMapper mapper, ICryptoHelper cryptoHelper)
        {
            _context = posDbContext;
            _mapper = mapper;
            _cryptoHelper = cryptoHelper;
        }


        public UserDto Add(UserDto dto)
        {
            if (_context.Users.Any(x => x.UserName == dto.UserName))
                return dto.AddError("Username already exists!");

            if (_context.Users.Any(x => x.UserName == dto.Email))
                return dto.AddError("Email is used before!");

            var entity = _mapper.Map<User>(dto);
            entity.Password = _cryptoHelper.Hash(entity.Password);
            _context.Users.Add(entity);
            _context.SaveChanges();
            dto = _mapper.Map<UserDto>(entity);
            return dto.AddSuccess("User added successfully!");
        }

        public Result Delete(Guid entityId)
        {
            var result = new Result();
            var entity = _context.Users.SingleOrDefault(x => x.Id == entityId);
            if (entity == null)
                return result.AddError("User not found!");

            _context.Users.Remove(entity);
            _context.SaveChanges();
            return result.AddSuccess();
        }

        public Result Update(UserDto dto)
        {
            var result = new Result();
            var entity = _context.Users.SingleOrDefault(x => x.Id == dto.Id);
            if (entity == null)
                return result.AddError("User not found!");

            var newEntity = _mapper.Map<User>(dto);
            _context.Users.Update(newEntity);
            _context.SaveChanges();
            return result.AddSuccess();
        }

        public UserDto Get(Guid id)
        {
            return _mapper.Map<UserDto>(_context.Users.AsNoTracking().SingleOrDefault(x => x.Id == id));
        }

        public UserDto GetUserByUserName(string userName)
        {
            return _mapper.Map<UserDto>(_context.Users.AsNoTracking().SingleOrDefault(x => x.UserName == userName));
        }

        public PagedResult<UserDto> GetWithPaging(int page, int pageSize, Expression<Func<User, bool>> predicate = null)
        {
            if (predicate == null) predicate = entity => true;
            var query = _context.Users.AsNoTracking().Where(predicate);

            var result = new PagedResult<UserDto> {CurrentPage = page, PageSize = pageSize, RowCount = query.Count()};
            result.PageCount = (int)Math.Ceiling((double)result.RowCount / pageSize);
 
            var skip = (page - 1) * pageSize;     
            var list = query.Skip(skip).Take(pageSize).ToList();
            foreach (var user in list)
                result.Results.Add(_mapper.Map<UserDto>(user));

            return result;
        }
    }
}
