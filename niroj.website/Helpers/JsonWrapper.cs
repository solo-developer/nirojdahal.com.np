using Newtonsoft.Json;

namespace niroj.website.Helpers
{
    public class JsonWrapper
    {
        public static string buildSuccessJson(object data)
        {
            var apiData = new { data };
            return JsonConvert.SerializeObject(apiData);
        }

        public static string buildErrorJson(string error)
        {
            var apiMessage = new { error,success=false };
            return JsonConvert.SerializeObject(apiMessage);
        }
    }
}
