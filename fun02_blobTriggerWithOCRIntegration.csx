using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Text;

public static async Task Run(Stream myBlob, string name, ILogger log)
{
    log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
    var array =   ToByteArrayAsync(myBlob);
    var result = await MakeRequest(array, log);
    return;
}

private static byte[] ToByteArrayAsync(Stream stream)
{
    Int32 length = stream.Length > Int32.MaxValue ? Int32.MaxValue : Convert.ToInt32(stream.Length);
    byte[] buffer = new Byte[length];
    stream.Read(buffer, 0, length);
    stream.Position = 0;
    return buffer;
}

private async static Task<HttpResponseMessage> MakeRequest(byte[] bytes,ILogger log)
{
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "<Vision API Subscription Key>");

            // Request parameters
            queryString["language"] = "unk";
            queryString["detectOrientation "] = "true";
            var uri = "https://westeurope.api.cognitive.microsoft.com/vision/v1.0/ocr?" + queryString;

            HttpResponseMessage response;

            // Request body
           
            //byte[] byteData = Encoding.UTF8.GetBytes("{\"url\":\"https://cms-assets.tutsplus.com/uploads/users/988/posts/20460/image/OCR%20Documents%20(1A).jpg\"}");
            
            HttpContent payload=new ByteArrayContent(bytes);
            payload.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response =  await client.PostAsync(uri, payload);
            /*
            using (var content = new ByteArrayContent(byteData))
            {
               content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
               response =  await client.PostAsync(uri, content);
            }*/
             using (HttpContent content = response.Content)
                {
                    
                    Task<string> result =  content.ReadAsStringAsync();
                    string res = result.Result;
                    log.LogInformation(res);
                }
            return response; 
}    
