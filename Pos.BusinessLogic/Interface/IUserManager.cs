using Pos.BusinessLogic.Dto;
using Pos.BusinessLogic.Interface.Common;
using Pos.DataAccess.Entities;

namespace Pos.BusinessLogic.Interface
{
    public interface IUserManager : ICommonOperation<UserDto, User>
    {
        UserDto GetUserByUserName(string userName);
    }
}
