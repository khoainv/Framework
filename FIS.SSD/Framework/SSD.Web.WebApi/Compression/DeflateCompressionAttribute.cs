#region

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using SSD.Framework;

#endregion

namespace SSD.Web.Compression
{
    public class DeflateCompressionAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actContext, CancellationToken cancellationToken)
        {
            base.OnActionExecutedAsync(actContext, cancellationToken);
            return ActionExecutedAsyncInternal(actContext, cancellationToken);
        }
        private async Task ActionExecutedAsyncInternal(HttpActionExecutedContext actContext, CancellationToken cancellationToken)
        {
            if (actContext.Response != null)
            {
                var content = actContext.Response.Content;
                var bytes = content == null ? null : content.ReadAsByteArrayAsync().Result;
                var zlibbedContent = bytes == null ? new byte[0] : await CompressionHelper.DeflateByteAsync(bytes);
                actContext.Response.Content = new ByteArrayContent(zlibbedContent);
                actContext.Response.Content.Headers.Remove(UGConstants.HTTPHeaders.ContentTypeJson);
                actContext.Response.Content.Headers.Add(UGConstants.HTTPHeaders.CONTENT_ENCODING, UGConstants.HTTPHeaders.ContentEncodingDeflate);
                actContext.Response.Content.Headers.Add(UGConstants.HTTPHeaders.CONTENT_TYPE, UGConstants.HTTPHeaders.ContentTypeJson);
            }
        }
    }
}
