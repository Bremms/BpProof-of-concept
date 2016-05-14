using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using RiotSharp;
using RiotSharp.MatchEndpoint;
namespace ProjectLeague.Hubs
{
    public class LeagueHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
        public void SendMatchData(long match_id, string groupname)
        {
            Groups.Add(Context.ConnectionId, groupname);
            var api = RiotApi.GetInstance("c888e0ff-55a2-4489-a4c7-e911c1d00730");
            var match = api.GetMatch(Region.euw, match_id, true);
            TimeSpan last = new TimeSpan();
            foreach (var frame in match.Timeline.Frames)
            {
                if (frame.Events != null)
                {

                    SendCoordinates(groupname, frame.ParticipantFrames);
                    for (int i = 0; i < frame.Events.Count; i++)
                    {
                        //System.Threading.Thread.Sleep(frame.Events[i].Timestamp - last);
                        last = frame.Events[i].Timestamp;
                        switch (frame.Events[i].EventType)
                        {
                            case EventType.BuildingKill:
                                Clients.Group(groupname).BuildingKill(frame.Events[i]);
                                break;
                            case EventType.ChampionKill:
                                Clients.Group(groupname).ChampionKill(frame.Events[i]);
                                break;
                            case EventType.EliteMonsterKill:
                                Clients.Group(groupname).EliteMonsterKill(frame.Events[i]);
                                break;
                            case EventType.ItemPurchased:
                                Clients.Group(groupname).ItemPurchased(frame.Events[i]);
                                break;
                            case EventType.ItemDestroyed:
                                Clients.Group(groupname).ItemDestroyed(frame.Events[i]);
                                break;
                            case EventType.ItemSold:
                                Clients.Group(groupname).ItemSold(frame.Events[i]);
                                break;
                            default:
                                break;

                        }

                    }
                }
            }
        }
        private void SendCoordinates(string groupname, Dictionary<String, ParticipantFrame> frames)
        {
            List<ParticipantFrame> parts = new List<ParticipantFrame>();
            foreach (var k in frames)
            {
                parts.Add(k.Value);
            }
            Clients.Group(groupname).Position(parts);
        }
    }
}