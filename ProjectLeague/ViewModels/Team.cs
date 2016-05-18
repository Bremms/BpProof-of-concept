using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectLeague.ViewModels
{
    public class Team
    {
        public List<Player> Players { get; set; }
        public string Teamname { get; set; }
        public Team()
        {
            Players = new List<Player>();
        }
        public Team(List<Player> players)
        {
            this.Players = players;
        }
        public void AddPlayer(string championName,int id=0)
        {
            Players.Add(new Player(id,championName));
        }
    }
}