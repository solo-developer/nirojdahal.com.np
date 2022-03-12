using Newtonsoft.Json;
using Personal.Domain.Configs;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace niroj.website.Helpers
{
    public class ReCaptchaClass
    {
        private static HttpClient _client = new HttpClient();
        public static async Task<string> Validate(RecaptchaConfiguration recaptchaConfig, string EncodedResponse)
        {
            ReCaptchaClass captchaResponse = new ReCaptchaClass();

            var GoogleReply = await _client.GetAsync(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", recaptchaConfig.SecretKey, EncodedResponse));

            string response = await GoogleReply.Content.ReadAsStringAsync();
            captchaResponse = JsonConvert.DeserializeObject<ReCaptchaClass>(response);
            return captchaResponse.Success.ToLower();
        }

        [JsonProperty("success")]
        public string Success
        {
            get { return _mSuccess; }
            set { _mSuccess = value; }
        }

        private string _mSuccess;
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes
        {
            get { return _mErrorCodes; }
            set { _mErrorCodes = value; }
        }


        private List<string> _mErrorCodes;
    }
}
