using Pos.BusinessLogic.Dto.Base;

namespace Pos.Api.ViewModel.Base
{
    public class BaseResponse<T> : Result
    {
        public T Data { get; set; } = default(T);
    }

    public class BaseResponse : Result
    {
        public object Data { get; set; }
    }
}
