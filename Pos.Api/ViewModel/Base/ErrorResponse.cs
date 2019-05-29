using Pos.BusinessLogic.Dto.Base;

namespace Pos.Api.ViewModel.Base
{
    public class ErrorResponse : Result
    {
        public string ErrorTraceId { get; set; }
    }
}
