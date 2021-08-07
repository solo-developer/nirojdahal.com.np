using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace niroj.website.Helpers
{
    public class ReCaptchaClass
    {
        public static string GetSecretKey() => "6LdIzHkaAAAAABJn5JCQdHcjAcDxlE9UZfEF1grq";
        private static HttpClient client = new HttpClient();
        public static async Task<string> Validate(string EncodedResponse)
        {
            ReCaptchaClass captchaResponse = new ReCaptchaClass();

            var GoogleReply =await client.GetAsync(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", GetSecretKey(), EncodedResponse));

            string response = await GoogleReply.Content.ReadAsStringAsync();
            captchaResponse = JsonConvert.DeserializeObject<ReCaptchaClass>(response);
            return captchaResponse.Success.ToLower();
        }

        [JsonProperty("success")]
        public string Success
        {
            get { return m_Success; }
            set { m_Success = value; }
        }

        private string m_Success;
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes
        {
            get { return m_ErrorCodes; }
            set { m_ErrorCodes = value; }
        }


        private List<string> m_ErrorCodes;
    }
}
