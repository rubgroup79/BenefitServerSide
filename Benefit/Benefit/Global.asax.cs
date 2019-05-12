using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Benefit.Models;
using Benefit.Controllers;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;

namespace Benefit
{
    public class WebApiApplication : System.Web.HttpApplication
    {

        static System.Timers.Timer timer = new System.Timers.Timer();
        static System.Timers.Timer timer2 = new System.Timers.Timer();
        string path = null;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            timer.Interval = 60000;
            timer.Elapsed += tm_Tick;
            path = Server.MapPath("/");
            StartTimer();
        }

        private void tm_Tick(object sender, ElapsedEventArgs e)
        {
            //EndTimer();


            //&& DateTime.Now.ToString("HH:mm tt") == "00:00"
            if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday  && DateTime.Now.ToString("HH") == "17")
            {
                //Console.Beep(300, 1000);
                DBservices db = new DBservices();
                List<Trainee> tl = db.GetLazyTrainees();

                for (int i = 0; i < tl.Count; i++)
                {
                    PushNotification pn = new PushNotification();
                    pn.To = tl[i].Token;
                    pn.Title = "benefit";
                    pn.Body = "hi " + tl[i].FirstName + " we've missed you!";
                    pn.Badge = 1;
                    this.PushNotification(pn);
                }

                
                List<Trainer> trl = db.GetLazyTrainers();
                for (int i = 0; i < tl.Count; i++)
                {
                    PushNotification pn = new PushNotification();
                    pn.To = trl[i].Token;
                    pn.Title = "benefit";
                    pn.Body = "hi " + trl[i].FirstName + " we've missed you!";
                    pn.Badge = 1;
                    this.PushNotification(pn);
                }

            }
        }
        
        public static void StartTimer()
        {
            timer.Enabled = true;
        }

        public static void EndTimer()
        {
            timer.Enabled = false;
        }


        public string PushNotification (PushNotification pnd)
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

    }


}
