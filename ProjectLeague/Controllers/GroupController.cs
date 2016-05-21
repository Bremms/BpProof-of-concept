using ProjectLeague.Models.DAL;
using ProjectLeague.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectLeague.Controllers
{
    public class GroupController : Controller
    {
        GroupRepo grpRepo;
        public GroupController()
        {
            DbEntitiesContext ctx = new DbEntitiesContext();
            grpRepo = new GroupRepo(ctx);
        }
        // GET: Group
        public ActionResult Index()
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
            return View(groupVms);
        }
    }
}