using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;


namespace Portal.Web
{
    public class Vimeo
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly string consumerKey = "a6038d30840fec02b6a31759c9556347c545865a";
        private static readonly string consumerSecret = "050991fa60574815cdb6eda3614d23d317d910f9";
        private static readonly string accessToken = "edeecb2fa793cc0d4a675a31de7913de";
        private static readonly string accessTokenSecret = "3eda8391024cec4c31f5c66404ca73b2a5f1ef6d";



        public void Test()
        {
            var baseUrl = "http://vimeo.com/api/rest/v2";
            var fullUrl = baseUrl + "?format=json&method=vimeo.test.login";

            var authorizationParts = new SortedDictionary<string, string>
            {
                { "oauth_consumer_key", consumerKey },
                { "oauth_nonce", GenerateNonce() },
                { "oauth_signature_method", "HMAC-SHA1" },
                { "oauth_token", accessToken },
                { "oauth_timestamp", GenerateTimeStamp() },
                //{ "oauth_verifier", verifier },
                { "oauth_version", "1.0" },
            };

            var parameterBuilder = new StringBuilder();
            foreach (var authorizationKey in authorizationParts)
            {
                parameterBuilder.AppendFormat("{0}={1}&", Uri.EscapeDataString(authorizationKey.Key), Uri.EscapeDataString(authorizationKey.Value));
            }
            parameterBuilder.Length--;
            string parameterString = parameterBuilder.ToString();

            var canonicalizedRequestBuilder = new StringBuilder();
            canonicalizedRequestBuilder.Append(HttpMethod.Post.Method);
            canonicalizedRequestBuilder.Append("&");
            canonicalizedRequestBuilder.Append(Uri.EscapeDataString(baseUrl));
            canonicalizedRequestBuilder.Append("&");
            canonicalizedRequestBuilder.Append(Uri.EscapeDataString(parameterString));

            string signature = ComputeSignature(consumerSecret, accessTokenSecret, canonicalizedRequestBuilder.ToString());
            authorizationParts.Add("oauth_signature", signature);

            var authorizationHeaderBuilder = new StringBuilder();
            authorizationHeaderBuilder.Append("OAuth ");
            foreach (var authorizationPart in authorizationParts)
            {
                authorizationHeaderBuilder.AppendFormat(
                    "{0}=\"{1}\", ", authorizationPart.Key, Uri.EscapeDataString(authorizationPart.Value));
            }
            authorizationHeaderBuilder.Length = authorizationHeaderBuilder.Length - 2;

            var request = new HttpRequestMessage(HttpMethod.Post, fullUrl);
            request.Headers.Add("Authorization", authorizationHeaderBuilder.ToString());


            var httpClient = new HttpClient();

            var result = httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead).Result;
            var response = result.Content.ReadAsStringAsync().Result;

        }

        private static string GenerateNonce()
        {
            return Guid.NewGuid().ToString("N");
        }

        private static string GenerateTimeStamp()
        {
            TimeSpan secondsSinceUnixEpocStart = DateTime.UtcNow - Epoch;
            return Convert.ToInt64(secondsSinceUnixEpocStart.TotalSeconds).ToString(CultureInfo.InvariantCulture);
        }

        private static string ComputeSignature(string consumerSecret, string tokenSecret, string signatureData)
        {
            using (var algorithm = new HMACSHA1())
            {
                algorithm.Key = Encoding.ASCII.GetBytes(
                    string.Format(CultureInfo.InvariantCulture,
                        "{0}&{1}",
                        Uri.EscapeDataString(consumerSecret),
                        string.IsNullOrEmpty(tokenSecret) ? string.Empty : Uri.EscapeDataString(tokenSecret)));
                byte[] hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(signatureData));
                return Convert.ToBase64String(hash);
            }
        }


    }
}