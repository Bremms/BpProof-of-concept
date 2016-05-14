using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectLeague.ViewModels
{
    public class Team
    {
        public List<string> Players { get; set; }
        public Team()
        {
            Players = new List<string>();
        }
        public Team(List<string> players)
        {
            this.Players = players;
        }
        public void AddPlayer(string player)
        {
            Players.Add(String.Format("http://ddragon.leagueoflegends.com/cdn/6.9.1/img/champion/{0}.png", player));
        }
    }
}