using System;

namespace Pos.Core.Interface
{
    public interface IStamp
    {
        DateTime CreatedDate { get; set; }
        Guid? CreatedUserId { get; set; }
        DateTime UpdatedDate { get; set; }
        Guid? UpdatedUserId { get; set; }
    }
}
