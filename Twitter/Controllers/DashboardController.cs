using System.Web.Mvc;
using Twitter.Models;

namespace Twitter.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        // GET: Account /Register 
        //public ActionResult StoryboardView()
        //{
        //    return PartialView("~/Views/Dashboard/_StoryboardView.cshtml", Following);
        //}
    }
}