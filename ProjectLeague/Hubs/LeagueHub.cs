using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using RiotSharp;
using RiotSharp.MatchEndpoint;
using ProjectLeague.Models.DAL;
using ProjectLeague.Models;

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
            //TODO
            if (stopCalled)
            {
                using (var ctx = new DbEntitiesContext())
                {
                    GroupRepo gRepo = new GroupRepo(ctx);
                    gRepo.RemoveClientFromGroup(Context.ConnectionId);
                    gRepo.SaveChanges();
                    //UserRepo uRepo = new UserRepo(ctx);
                    //var client = uRepo.FindClientById(Context.ConnectionId);
                    //var group_id = client.Group_id;
                    //var count = client.Group.Users.Count;
                    //uRepo.Delete(client);
                    //if (count == 1)
                    //{
                    //    var groupRepo = new GroupRepo(ctx);
                    //    var group = groupRepo.FindById(group_id);
                    //    groupRepo.Delete(group);
                    //    //TODO : group verwijderen
                    //}
                    //uRepo.SaveChanges();
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
                Clients.Caller.RetrieveMessage("Your nick has changed to: " + nickname);
            }
        }
        public Task GetConnectedUsers(string groupName)
        {
            using (var ctx = new DbEntitiesContext())
            {
                var groupRepo = new GroupRepo(ctx);
                var grp = groupRepo.FindByName(groupName);
                string content = "Users Online: " + grp.Users.Count + " ";
                groupRepo.SaveChanges();
                foreach (var user in grp.Users)
                {
                    content += user.Nickname + " ";
                }
                return Clients.Caller.RetrieveMessage(content);
            }
        }
        public void Hello()
        {
            Clients.All.hello();
        }
        public Task Join(string groupname, string username)
        {
            Groups.Add(Context.ConnectionId, groupname);
            var c = Context;
            using (var ctx = new DbEntitiesContext())
            {
                GroupRepo gRepo = new GroupRepo(ctx);
                //UserRepo uRepo = new UserRepo(ctx);
                var group = gRepo.FindByName(groupname);
                gRepo.AddClientToGroup(groupname, new Models.Client() { Connection_id = Context.ConnectionId, Group = group, Nickname = username });
                //uRepo.add(new Models.Client() { Connection_id = Context.ConnectionId, Group = group, Nickname = username });
                gRepo.SaveChanges();
                return Clients.Caller.RetrieveMessage("Server> type /start to start the game!");
                
            }
           
        }
        public Task SendMessage(string content, string grpName)
        {
            return Clients.Group(grpName).RetrieveMessage(content);
        }
        public Task SendMatchData(long match_id, string groupname)
        {
            Task t = Task.Factory.StartNew(() =>
            {
                //Groups.Add(Context.ConnectionId, groupname);
                using (var ctx = new DbEntitiesContext())
                {
                    var groupRepo = new GroupRepo(ctx);
                    var group = groupRepo.FindByName(groupname);
                    group.IsJoinable = false;
                    groupRepo.saveChangesAsync();
                }
                var api = RiotApi.GetInstance(Config.API_KEY);
                var match = api.GetMatch(Region.euw, match_id, true);
                TimeSpan last = new TimeSpan();
                foreach (var frame in match.Timeline.Frames)
                {
                    if (frame.Events != null)
                    {

                        foreach (var event_ in frame.Events)
                        // for (int i = 0; i < frame.Events.Count; i++)
                        {
                            var time = event_.Timestamp - last;
                            //if(time.Seconds>0.1)
                            //{
                            //    System.Threading.Thread.Sleep(1);
                            //}
                            //else
                            //{
                            //    System.Threading.Thread.Sleep(frame.Events[i].Timestamp - last);
                            //}

                            // if (frame.Events[i].Timestamp - last>new TimeSpan())
                            CalculateTime(event_, last);
                            last = event_.Timestamp;
                            switch (event_.EventType)
                            {

                                case EventType.BuildingKill:
                                    Clients.Group(groupname).BuildingKill(event_.Position);
                                    break;
                                case EventType.ChampionKill:
                                    Clients.Group(groupname).ChampionKill(event_.KillerId, event_.VictimId, event_.AssistingParticipantIds, event_.Position);
                                    break;
                                case EventType.EliteMonsterKill:
                                    Clients.Group(groupname).EliteMonsterKill(event_.KillerId, event_.MonsterType, event_.Position);
                                    break;
                                case EventType.ItemPurchased:
                                    Clients.Group(groupname).ItemPurchased(event_.ParticipantId, event_.ItemId);
                                    break;
                                case EventType.ItemDestroyed:
                                    Clients.Group(groupname).ItemDestroyed(event_.ParticipantId, event_.ItemId);
                                    break;
                                case EventType.ItemUndo:
                                    Clients.Group(groupname).ItemDestroyed(event_.ParticipantId, event_.ItemBefore);
                                    break;
                                case EventType.ItemSold:
                                    Clients.Group(groupname).ItemDestroyed(event_.ParticipantId, event_.ItemId);
                                    break;
                                default:
                                    break;

                            }

                        }
                            SendCoordinates(groupname, frame.ParticipantFrames);
                    }
                }
            });
            return t;

        }
        private void CalculateTime(Event event_,TimeSpan last)
        {
            if(event_.EventType== EventType.BuildingKill|| event_.EventType == EventType.ChampionKill || event_.EventType == EventType.EliteMonsterKill || event_.EventType == EventType.ItemPurchased || event_.EventType == EventType.ItemDestroyed || event_.EventType == EventType.ItemUndo || event_.EventType == EventType.ItemSold)
            {
                System.Threading.Thread.Sleep(event_.Timestamp - last);
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