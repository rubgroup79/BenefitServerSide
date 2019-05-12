using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;
using Benefit.Models;

namespace Benefit.Controllers
{
    public class PushController : ApiController
    {
        [Route("sendpushnotification")]
        public string Post([FromBody]PushNotification pnd)
        //public string Post(string To, string Title, string Body, int Badge)
        {
            // Create a request using a URL that can receive a post.   
            WebRequest request = WebRequest.Create("https://exp.host/--/api/v2/push/send");
            // Set the Method property of the request to POST.  
            request.Method = "POST";
            // Create POST data and convert it to a byte array.  


            var objectToSend = new
            {
                to = pnd.To,
                title = pnd.Title,
                body = pnd.Body,
                badge = pnd.Badge,
                //data = pnd.data//new { name = "nir", grade = 100 }
            };

            string postData = new JavaScriptSerializer().Serialize(objectToSend);

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.  
            request.ContentType = "application/json";
            // Set the ContentLength property of the WebRequest.  
            request.ContentLength = byteArray.Length;
            // Get the request stream.  
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.  
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.  
            dataStream.Close();
            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            string returnStatus = ((HttpWebResponse)response).StatusDescription;
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.  
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.  
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.  
            string responseFromServer = reader.ReadToEnd();
            // Display the content.  
            //Console.WriteLine(responseFromServer);
            // Clean up the streams.  
            reader.Close();
            dataStream.Close();
            response.Close();

            return "success:) --- " + responseFromServer + ", " + returnStatus;
        }
        // https://www.pubnub.com/blog/react-native-push-notifications-ios-android/
        // https://docs.expo.io/versions/latest/guides/push-notifications/
        //https://levelup.gitconnected.com/react-native-adding-push-notifications-to-your-app-with-expo-8e4b659ddbfb
        //https://www.youtube.com/watch?v=kZqzAPqRbS8 
        // GET: api/Push
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Push/5
        public string Get(int id)
        {
            return "value";
        }

        //// POST: api/Push
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT: api/Push/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Push/5
        public void Delete(int id)
        {

        }
    }
    

}
