using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectLeague.ViewModels
{
    public class Teamcontainer
    {
        public List<Team> Teams { get; set; }
        public long MatchId { get; set; }
        public string SummonerName { get; set; }
        public string GroupName { get; set; }
        public string UserName { get; set; }
        public Teamcontainer()
        {

        }
    }
}