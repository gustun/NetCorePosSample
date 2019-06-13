using Pos.BusinessLogic.Dto.Common;

namespace Pos.Api.ViewModel.Requests
{
    public class GetProductRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public ProductFilterDto FilterOptions { get; set; }
    }
}
