using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectLeague.ViewModels
{
    public class Player
    {
        public int Id { get; set; }
        public string Champion { get; set; }
        public string ChampionUrl { get; set; }
        public Player()
        {

        }
        public Player(int id, string champion)
        {
            this.Id = id;
            this.Champion = champion;
            ChampionUrl = String.Format("http://ddragon.leagueoflegends.com/cdn/6.9.1/img/champion/{0}.png", champion);
        }
        public Player(string champion)
        {
            this.ChampionUrl = String.Format("http://ddragon.leagueoflegends.com/cdn/6.9.1/img/champion/{0}.png", champion);
        }
    }
}