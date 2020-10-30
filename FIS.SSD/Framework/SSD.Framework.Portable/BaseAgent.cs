using System.Linq;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using System.Net.Http.Headers;
using System.Text;
using System.IO;
using Ionic.Zlib;
using SSD.Framework.Exceptions;
using SSD.Framework.Models;
using SSD.Framework.Helpers;

namespace SSD.Framework
{
    public abstract class BaseAgent<T>:Singleton<T> where T:class, new()
    {
        static HttpClient httpClient;
        public HttpClient HTTPClient
        {
            get
            {
                if (httpClient == null)
                {
                    if (string.IsNullOrWhiteSpace(Settings.UrlBase) 
                        || string.IsNullOrWhiteSpace(Settings.IoTClientId)
                        || string.IsNullOrWhiteSpace(Settings.IoTClientSecret))
                        throw new BusinessException(ErrorCode.InitSettings, "Not Init UGStartup, Please init UGStartup");
                    httpClient = new HttpClient();
                    httpClient.BaseAddress = new Uri(Settings.UrlBase);
                    httpClient.DefaultRequestHeaders.Accept.Add(
                       new MediaTypeWithQualityHeaderValue(UGConstants.HTTPHeaders.ContentTypeJson));
                    httpClient.DefaultRequestHeaders.Add(UGConstants.HTTPHeaders.IOT_CLIENT_ID, Settings.IoTClientId);
                    httpClient.DefaultRequestHeaders.Add(UGConstants.HTTPHeaders.IOT_CLIENT_SECRET, Settings.IoTClientSecret);

                    //// The following line sets a "User-Agent" request header as a default header on the HttpClient instance.
                    //// Default headers will be sent with every request sent from this HttpClient instance.
                    //httpClient.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("Sample", "v8"));
                }
                
                return httpClient;
            }
        }
        public TResult GetDataJson<TResult>(string MsgJson) where TResult : class, new()
        {
            try
            {
                var tmp = JsonConvert.DeserializeObject<TResult>(MsgJson, new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Error,
                    Error = ErrorHandler
                });
                return tmp;
            }
            catch (Exception ex)
            {
                //TODO
                //log error
                throw new JsonParserException("Lỗi dữ liệu Json: " + ex.Message);// (code, msg);  Base Exception header throw new WebServiceException(ex, "Valide Json Object not mask");
            }
        }
        private static void ErrorHandler(object x, Newtonsoft.Json.Serialization.ErrorEventArgs error)
        {
            //throw error.ErrorContext.Error;
            error.ErrorContext.Handled = false;
        }
        //Use fastest JSON serializer https://github.com/kevin-montrose/Jil Not support .Net porable
        public string SetDataJson(object value)
        {
            return JsonConvert.SerializeObject(value);

            //var msg = string.Empty;
            //using (var output = new StringWriter())
            //{
            //    JSON.Serialize(value, output);
            //    msg = output.ToString();
            //}
            //return msg;
        }
        protected async Task<TResult> ExecuteAsync<TResult>(RequestBase request)
           // where TResult : new()
        {
            string publicKey = "";
            BaseMessage msg = new BaseMessage(publicKey,"",ErrorCode.IsSuccess,"");
            msg = PackageDataWWithSecurity(msg, request.Data);

            var data = SetDataJson(msg);


            if (!string.IsNullOrWhiteSpace(request.UGToken))
            {
                if (Settings.IsUsingSSO)
                {
                    HTTPClient.SetBearerToken(request.UGToken);
                    HTTPClient.DefaultRequestHeaders.Add(UGConstants.ClaimTypes.PreferredUserName, Settings.UserName);
                }
                else
                {
                    if (HTTPClient.DefaultRequestHeaders.Contains(UGConstants.HTTPHeaders.TOKEN_NAME))
                        HTTPClient.DefaultRequestHeaders.Remove(UGConstants.HTTPHeaders.TOKEN_NAME);
                    HTTPClient.DefaultRequestHeaders.Add(UGConstants.HTTPHeaders.TOKEN_NAME, request.UGToken);
                }
            }
            var response = new HttpResponseMessage();
            //try
            //{
            if(request.Method==HttpMethod.Post)
                response = await HTTPClient.PostAsync(request.Resource, new StringContent(data, Encoding.UTF8, UGConstants.HTTPHeaders.ContentTypeJson));
            else 
            if (request.Method == HttpMethod.Put)
                response = await HTTPClient.PutAsync(request.Resource, new StringContent(data, Encoding.UTF8, UGConstants.HTTPHeaders.ContentTypeJson));
            else 
                if (request.Method == HttpMethod.Get)
                    response = await HTTPClient.GetAsync(request.Resource);
            //}
            //catch (HttpRequestException ex)
            //{
            //    throw new HTTPException(System.Net.HttpStatusCode.Unauthorized, ex.Message);
            //}

            if (response.IsSuccessStatusCode)
            {
                string contentStr = string.Empty;

                IEnumerable<string> lst;
                if (response.Content.Headers!=null
                    &&response.Content.Headers.TryGetValues(UGConstants.HTTPHeaders.CONTENT_ENCODING, out lst)
                    && lst.First().ToLower() == UGConstants.HTTPHeaders.ContentEncodingDeflate)
                {
                    var bj = await response.Content.ReadAsByteArrayAsync();
                    using (var inStream = new MemoryStream(bj))
                    {
                        using (var bigStreamsss = new DeflateStream(inStream, CompressionMode.Decompress, true))
                        {
                            contentStr = await (new StreamReader(bigStreamsss)).ReadToEndAsync();
                            //using (var bigStreamOut = new MemoryStream())
                            //{
                            //    bigStreamsss.CopyTo(bigStreamOut);
                            //    contentStr = Encoding.UTF8.GetString(bigStreamOut.ToArray(), 0, bigStreamOut.ToArray().Length);
                            //}
                        }
                    }
                }
                else contentStr = await response.Content.ReadAsStringAsync();

                BaseMessage responseContent = GetDataJson<BaseMessage>(contentStr);
                
                if (responseContent.ErrorCode!=ErrorCode.IsSuccess)
                    throw new BusinessException(responseContent.ErrorCode, responseContent.ErrorMsg);

                string error;
                CheckSignature(responseContent,out error);

                return responseContent.GetData <TResult>();
            }
            else
            {
                throw new UnknownException(response.StatusCode.ToString());
            }
        }
        protected bool CheckSignature(BaseMessage msg,out string error)
        {
            bool result = true; error = string.Empty;
            if (!string.IsNullOrWhiteSpace(msg.MsgJson))
            {
                string bankPublicKeyReplaced = msg.PublicKey;
                bankPublicKeyReplaced = bankPublicKeyReplaced.Replace("-----BEGIN CERTIFICATE-----", "");
                bankPublicKeyReplaced = bankPublicKeyReplaced.Replace("-----END CERTIFICATE-----", "");

                //Kiểm tra PublicKey trong CSDL trung tâm
                if (UGConstants.Security.IS_VERIFY_CRL && VerifyCRL(bankPublicKeyReplaced))
                {
                    result = false;
                    error = UGConstants.Security.MsgValidPublicKey;
                }

                //Kiểm tra chữ ký điện tử
                if (UGConstants.Security.IS_VERIFY_SIGNATURE)// && RSAEngine.VerifySign(msg.MsgJson, bankPublicKeyReplaced, msg.Signature))
                {
                    result = false;
                    error = UGConstants.Security.MsgValidSignature;
                }
            }
            return result;
        }
        private bool VerifyCRL(string publicKey)
        {
            bool isPass = true;
            //string serialNumber = GetSerialNumber(publicKey);
            //isPass = LocationStoreBiz.Instance.VerifyCRL(serialNumber);
            return isPass;
        }

        protected BaseMessage PackageDataWWithSecurity(BaseMessage msg, object value)
        {
            msg.ErrorCode = ErrorCode.IsSuccess;
            msg.ErrorMsg = "Thành công";
            msg.SetData(value);

            //string bankPublicKeyReplaced = msg.PublicKey;
            //bankPublicKeyReplaced = bankPublicKeyReplaced.Replace("-----BEGIN CERTIFICATE-----", "");
            //bankPublicKeyReplaced = bankPublicKeyReplaced.Replace("-----END CERTIFICATE-----", "");
            ////Bat dau ma hoa message
            //if (UGConstants.Security.IS_ENCRYPT)
            //    msg.MsgJson = RSAEngine.EncryptData(msg.MsgJson, bankPublicKeyReplaced);

            //string sign = string.Empty;
            //if (UGConstants.Security.IS_VERIFY_SIGNATURE)
            //    sign = RSAEngine.SignData(msg.MsgJson);
            ////Append sign vao xmlMsg
            //msg.Signature = sign;

            ////Set publicKey Center
            //msg.PublicKey = GetPublicKey();

            return msg;
        }
    }
}
