using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectLeague.ViewModels
{
    public class Game
    {
        public string ChampionName { get; set; }
        public long GameId { get; set; }
        public DateTime Created { get; set; }
        public TimeSpan Duration { get; set; }
        public bool Won { get; set; }
        public int Kills { get; set; }
        public int Assists { get; set; }
        public int Deaths { get; set; }
        public string url { get; set; }
        public string Title { get; set; }
        public List<Team> Teams { get; set; }
        public Game()
        {

        }
        public Game(string name,string title,long id,DateTime created,TimeSpan duration,bool won,int kills,int assists,int deaths,List<Team> teams)
        {
            this.ChampionName = name;
            this.Title = title;
            this.GameId = id;
            this.Created = created;
            this.Duration = duration;
            this.Won = won;
            this.Kills = kills;
            this.Assists = assists;
            this.Deaths = deaths;
            url = String.Format("http://ddragon.leagueoflegends.com/cdn/6.9.1/img/champion/{0}.png", name);
            this.Teams = teams;
        }
    }
}