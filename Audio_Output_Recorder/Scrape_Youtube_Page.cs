using System;
using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Audio_Output_Recorder
{
    class Scrape_Youtube_Page
    {
        public static string googleSearchLink = "google.com/search?q=";

        private void Get_Name_of_ClickedItem()
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;

            string htmlContent;

            using (var client = new WebClient())
            {
                //htmlContent = client.DownloadString(url);
            }

            var htmlDoc = new HtmlDocument();
            //htmlDoc.LoadHtml(htmlContent);

            // Example: Extract the video title
            string title = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='title-text']")?.InnerText;

            Console.WriteLine($"Title: {title}");
        }
    }


    
}
