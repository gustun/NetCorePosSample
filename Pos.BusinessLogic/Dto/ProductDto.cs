using System;
using System.Collections.Generic;
using Pos.BusinessLogic.Dto.Base;

namespace Pos.BusinessLogic.Dto
{
    public class ProductDto : Result
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public UserDto CreatedUser { get; set; }

        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedUserId { get; set; }
        public UserDto UpdatedUser { get; set; }

        public ICollection<OrderProductDto> OrderProducts { get; set; }
    }
}
