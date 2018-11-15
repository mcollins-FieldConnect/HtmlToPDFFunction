using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using OpenHtmlToPdf;

namespace HtmlToPDFFunction
{
    public static class HtmlToPDF
    {
        [FunctionName("HtmlToPDF")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string html = await req.Content.ReadAsStringAsync();

            var pdf = Pdf
                .From(html)
                .Content();

            log.Info($"PDF Generated. Length={pdf.Length}");

            var res = new HttpResponseMessage(HttpStatusCode.OK);
            res.Content = new ByteArrayContent(pdf);
            res.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            res.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");

            return res;
        }
    }
}
