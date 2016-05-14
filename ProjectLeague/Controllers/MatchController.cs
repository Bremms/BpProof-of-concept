using ProjectLeague.ViewModels;
using RiotSharp;
using RiotSharp.StaticDataEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ProjectLeague.Controllers
{
    public class MatchController : Controller
    {
        private RiotApi riotApi;
        private StaticRiotApi staticApi;
        private ChampionListStatic allChampions;
        public MatchController()
        { 
            riotApi = RiotApi.GetInstance("c888e0ff-55a2-4489-a4c7-e911c1d00730");
            staticApi = StaticRiotApi.GetInstance("c888e0ff-55a2-4489-a4c7-e911c1d00730");
            allChampions = staticApi.GetChampions(Region.euw);
        }
        // GET: Match
        public ActionResult SelectSummoner(CreateGroup group)
        { 
            return View(new SelectSummoner() { GroupName = group.Groupname});
        }
        [HttpPost]
        public ActionResult SelectSummoner(SelectSummoner match)
        {
            try
            {
                var summoner = riotApi.GetSummoner(Region.euw, match.SummonerName);
                var matches = riotApi.GetRecentGames(Region.euw,summoner.Id).Where(w=>w.GameMode == GameMode.Classic);
                var allChampions = staticApi.GetChampions(Region.euw);
                var games = new List<Game>();
                foreach(var m in matches)
                {
                    var champion = allChampions.Champions.Where(w => w.Value.Id == m.ChampionId).FirstOrDefault().Value;
                    games.Add(new Game(champion.Key,champion.Title, m.GameId, m.CreateDate,m.Statistics.TimePlayed,m.Statistics.Win,m.Statistics.ChampionsKilled,m.Statistics.NumDeaths,m.Statistics.Assists,makeTeams(m)));
                }
                return View("SelectMatch",new GameContainer(games,match.SummonerName,match.GroupName));
            }
            catch(RiotSharpException ex)
            {
                return View("SelectSummoner");
            }
            
        }
        public ActionResult SelectMatch(long matchid, string sumName,string grpName)
        {
            return View("Match");
        }
        private List<Team> makeTeams(RiotSharp.GameEndpoint.Game game)
        {
            Team t1 = new Team();
            Team t2 = new Team();
            foreach(var player in game.FellowPlayers)
            {
                var championTemp = allChampions.Champions.Where(w => w.Value.Id == player.ChampionId).FirstOrDefault().Value;
                if (player.TeamId == 100)
                {
                    t1.AddPlayer(championTemp.Key);
                }
                else
                {
                    t2.AddPlayer(championTemp.Key);
                }
            }
            var champion = allChampions.Champions.Where(w => w.Value.Id == game.ChampionId).FirstOrDefault().Value;
            if (game.TeamId == 100)
            {
                t1.AddPlayer(champion.Key);
            }
            else
            {
                t2.AddPlayer(champion.Key);
            }
            return new List<Team>() { t1, t2 };
        }
    }
} 