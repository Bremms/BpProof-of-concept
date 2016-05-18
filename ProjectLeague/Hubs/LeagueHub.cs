using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using RiotSharp;
using RiotSharp.MatchEndpoint;
using ProjectLeague.Models.DAL;

namespace ProjectLeague.Hubs
{
    public class LeagueHub : Hub
    {
        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
            {
                using (var ctx = new DbEntitiesContext())
                {
                    UserRepo uRepo = new UserRepo(ctx);
                    var client = uRepo.FindClientById(Context.ConnectionId);
                    var group_id = client.Group_id;
                    var count = client.Group.Users.Count;
                    uRepo.Delete(client);
                    if (count==1)
                    {
                        var groupRepo = new GroupRepo(ctx);
                        var group = groupRepo.FindById(group_id);
                        groupRepo.Delete(group);
                        //TODO : group verwijderen
                    }
                    uRepo.SaveChanges();
                }
            }
            return base.OnDisconnected(stopCalled);
        }
        public void ChangeNickname(string nickname)
        {
            using (var ctx = new DbEntitiesContext())
            {
                UserRepo uRepo = new UserRepo(ctx);
                var client = uRepo.FindClientById(Context.ConnectionId);
                client.Nickname = nickname;
                uRepo.SaveChanges();
            }
        }
        public Task GetConnectedUsers(string groupName)
        {
            using (var ctx = new DbEntitiesContext())
            {
                var groupRepo = new GroupRepo(ctx);
                var grp = groupRepo.FindByName(groupName);
                string content = "Users Online: " + grp.Users.Count + " ";
                foreach(var user in grp.Users)
                {
                    content += user.Nickname + " ";
                }
                return Clients.Group(groupName).RetrieveMessage(content);
            }
        }
        public void Hello()
        {
            Clients.All.hello();
        }
        public Task Join(string groupname,string username)
        {
            Groups.Add(Context.ConnectionId, groupname);
            var c = Context;
            using (var ctx = new DbEntitiesContext())
            {
                GroupRepo gRepo = new GroupRepo(ctx);
                UserRepo uRepo = new UserRepo(ctx);
                var group = gRepo.FindByName(groupname);
                uRepo.add(new Models.Client() { Connection_id = Context.ConnectionId, Group = group ,Nickname = username});

                return uRepo.SaveChangesAsync();
            }
        }
        public Task SendMessage(string content,string grpName)
        {
            return Clients.Group(grpName).RetrieveMessage(content);
        }
        public Task SendMatchData(long match_id, string groupname)
        {
            Task t = Task.Factory.StartNew(() =>
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
                        System.Threading.Thread.Sleep(frame.Events[i].Timestamp - last);
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
                                Clients.Group(groupname).ItemPurchased(frame.Events[i].ParticipantId,frame.Events[i].ItemId);
                                break;
                            case EventType.ItemDestroyed:
                                Clients.Group(groupname).ItemDestroyed(frame.Events[i].ParticipantId,frame.Events[i].ItemId);
                                break;
                            case EventType.ItemSold:
                                Clients.Group(groupname).ItemDestroyed(frame.Events[i].ParticipantId,frame.Events[i].ItemId);
                                break;
                            default:
                                break;

                        }

                    }
                }
            }
            });
            return t;
           
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