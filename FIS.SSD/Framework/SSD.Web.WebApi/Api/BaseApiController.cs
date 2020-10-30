using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using SSD.Framework;
using SSD.Framework.Exceptions;
using SSD.Web.Security;
using SSD.Web.Models;
using SSD.Framework.Cryptography;
using SSD.Framework.Extensions;
using SSD.Web.Identity;
using System.Web;
using SSD.Framework.Models;

namespace SSD.Web.Api
{
    //Check Signature
    public class BaseApiController : ApiController
	{
        
        public override async Task<HttpResponseMessage> ExecuteAsync(System.Web.Http.Controllers.HttpControllerContext controllerContext, System.Threading.CancellationToken cancellationToken)
        {
            var request = controllerContext.Request;

            //chk Post and put
            if (request.Method != HttpMethod.Post && request.Method != HttpMethod.Put)
            {
                HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, "System only support Post and Put method.");
                return reply;//Task.FromResult(reply);
            }

            ////Check obj
            var result = await request.Content.ReadAsStringAsync();
            try
            {
                //var msg = JsonConvert.DeserializeObject<BaseMessage>(result);
                var msg = (BaseMessage)JsonConvert.DeserializeObject(result, typeof(BaseMessage), new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Error,
                    Error = ErrorHandler
                });
                //check PublicKey, Signature
                if (UGConstants.Security.USING_SIGNATURE)
                {
                    string error;
                    var chk = CheckSignature(msg, out error);
                    if (!chk)
                    {
                        var resultMsg = new BaseMessage("", "", ErrorCode.Signature, error);
                        HttpResponseMessage reply = request.CreateResponse<BaseMessage>(HttpStatusCode.OK, resultMsg);
                        return reply;
                    }
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.Unused, "Input para only support BaseMessage", ex);
                return reply;// Task.FromResult(reply);
            }

            return await base.ExecuteAsync(controllerContext, cancellationToken);
        }
        private static void ErrorHandler(object x, Newtonsoft.Json.Serialization.ErrorEventArgs error)
        {
            error.ErrorContext.Handled = false;
        }
        protected string DecryptData(string msgJson)
        {
            ////Giải mã dữ liệu
            //if (UGConstants.Security.IS_ENCRYPT)
            //    return RSAEngine.DecryptData(msgJson);

            return msgJson;
        }
        private bool CheckSignature(BaseMessage msg, out string error)
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
                if (UGConstants.Security.IS_VERIFY_SIGNATURE && RSAEngine.VerifySign(msg.MsgJson, bankPublicKeyReplaced, msg.Signature))
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
        protected BaseMessage GetBaseMessage(BaseException ex)
        {
            try
            {
                //LogErrorBiz.Instance.Insert(new LogError() { ErrorCode = ex.ErrorNumber.ToString(), ErrorMsg = ex.ToString() });
                return new BaseMessage("", "", ex.ErrorNumber, ex.ErrorMsg);
            }
            catch (Exception e)
            {
                //Loi ko log duoc vao db
                return new BaseMessage("", "", SSD.Framework.Exceptions.ErrorCode.Unknown, e.Message);
            }
        }
    }
}
