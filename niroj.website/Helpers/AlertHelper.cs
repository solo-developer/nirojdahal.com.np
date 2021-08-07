using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace niroj.website.Helpers
{
    public class AlertHelper
    {
        public static void setMessage(Controller controller, string message, MessageType message_type = MessageType.success)
        {
            Alert alert = new Alert();
            alert.message = message;
            alert.message_type = message_type.ToString();
            controller.TempData["message"] =JsonConvert.SerializeObject(alert);
        }
    }
}
