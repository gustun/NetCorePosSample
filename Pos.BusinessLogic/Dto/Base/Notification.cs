using Pos.Core.Enum;

namespace Pos.BusinessLogic.Dto.Base
{
    public class Notification
    {
        public ENotificationType NotificationType { get; set; }
        public string Message { get; set; }
    }
}
