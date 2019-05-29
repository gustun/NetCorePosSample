using System.Collections.Generic;
using System.Linq;
using Pos.Core.Enum;

namespace Pos.BusinessLogic.Dto.Base
{
    public class Result
    {
        public bool IsSuccess => Messages.All(x => x.NotificationType != ENotificationType.Error);
        public List<Notification> Messages { get; set; } = new List<Notification>();

        public void AddNotification(string message, ENotificationType type)
        {
            Messages.Add(new Notification
            {
                Message = message,
                NotificationType = type
            });
        }
    }

    public static class ResultExtensions
    {
        public static T AddSuccess<T>(this T result, string message = "Successfully completed the process!") where T : Result
        {
            if (result == null) return default(T);
            result.AddNotification(message, ENotificationType.Success);
            return result;
        }

        public static T AddError<T>(this T result, string message) where T : Result
        {
            if (result == null) return default(T);
            result.AddNotification(message, ENotificationType.Error);
            return result;
        }

        public static T AddInfo<T>(this T result, string message) where T : Result
        {
            if (result == null) return default(T);
            result.AddNotification(message, ENotificationType.Info);
            return result;
        }

        public static T AddWarning<T>(this T result, string message) where T : Result
        {
            if (result == null) return default(T);
            result.AddNotification(message, ENotificationType.Warning);
            return result;
        }

        public static T Merge<T>(this T result, T resultParam) where T : Result
        {
            if (result == null) return default(T);
            result.Messages.AddRange(resultParam.Messages);
            return result;
        }
    }
}
