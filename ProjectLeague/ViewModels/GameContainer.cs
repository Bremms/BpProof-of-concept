using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectLeague.ViewModels
{
    public class GameContainer
    {
        public List<Game> Games { get; set; }
        public string SummonerName { get; set; }
        public string GroupName { get; set; }
        public GameContainer()
        {

        }
        public GameContainer(List<Game> games, string sumName,string groupName)
        {
            this.Games = games;
            this.SummonerName = sumName;
            this.GroupName = groupName;
        }
    }
}