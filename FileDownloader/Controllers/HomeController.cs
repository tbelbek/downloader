using Flurl.Http;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebUI.Helpers;

namespace FileDownloader.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Download()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Download(DownloadData data)
        {
            var service = new SmtpService();

            if (data.Path == string.Empty)
            {
                return View("Index");
            }

            try
            {
                ViewBag.Info = "Download started";
                if (string.IsNullOrEmpty(data.Path))
                {
                    data.Path = ConfigurationSettings.AppSettings["DownloadDefaultPath"];
                }
                await data.Url.DownloadFileAsync(data.Path);
                service.SendMail("Info", $"Download Completed for {data.Url}", "destructer9@gmail.com");
            }
            catch (Exception ex)
            {
                ViewBag.Info = "Download error";
                service.SendMail("Info", $"Download Failed for {data.Url}, Exception {ex.Message}", "destructer9@gmail.com");
            }
            return View("Index");
        }
    }
    public class DownloadData
    {
        public string Url { get; set; }
        public string Password { get; set; }
        public string Path { get; set; }
    }
}