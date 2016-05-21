using Microsoft.AspNet.SignalR.Client;
using ProjectLeague.Models.DAL;
using ProjectLeague.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProjectLeague.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
                return View(new CreateGroup());
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Index(CreateGroup group)
        {
            //save groupname
            using (var ctx = new DbEntitiesContext())
            {
                GroupRepo gRepo = new GroupRepo(ctx);
                var grp = await gRepo.FindByNameAsync(group.Groupname);
                if(grp==null)
                {
                    gRepo.AddGroup(group.Groupname);
                    await gRepo.saveChangesAsync();
                    //var u = User;
                    //var hubConn = new HubConnection("http://localhost:50800/signalr");
                    //var leagueHub = hubConn.CreateHubProxy("LeagueHub");
                    //await hubConn.Start();
                    //await Task.Run(()=>leagueHub.Invoke("Join",group.Groupname).Wait());
                }
                else
                {
                    TempData["error"] = "Name already exist";
                    return RedirectToAction("Index");
                }
               
            }
            return RedirectToAction("SelectSummoner", "Match", group);
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
    }
}