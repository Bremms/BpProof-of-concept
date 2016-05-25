using ProjectLeague.Models.DAL;
using ProjectLeague.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectLeague.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        GroupRepo grpRepo;
        public GroupController()
        {
            DbEntitiesContext ctx = new DbEntitiesContext();
            grpRepo = new GroupRepo(ctx);
        }
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
                if (grp == null)
                {
                    gRepo.AddGroup(group.Groupname);
                    await gRepo.saveChangesAsync();
                }
                else
                {
                    TempData["error"] = "Name already exist";
                    return RedirectToAction("Index");
                }

            }
            return RedirectToAction("SelectSummoner", "Match", group);
        }
        // GET: Group
        public ActionResult GetAllGroups()
        {
            var groups = grpRepo.FindAll().Where(g => g.IsJoinable == true && g.Users.Count >= 1).ToList();
            var groupVms = new List<GroupVm>();
            if(groups!=null)
            {
                foreach(var g in groups)
                {
                    groupVms.Add(new GroupVm() { Name = g.Name, UserCount = g.Users.Count, MatchId = g.Match_id });
                }
            }
            return View("groups",groupVms);
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