using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ProjectLeague.Models.DAL
{
    public class GroupRepo
    {
        private DbEntitiesContext context;
        private DbSet<Group> groups;
        public GroupRepo(DbEntitiesContext ctx)
        {
            this.context = ctx;
            this.groups = ctx.Groups;
        }
        public void AddClientToGroup(string groupname, Client c)
        {
            Group g = this.FindByName(groupname);
            g.AddUser(c);
            context.SaveChanges() ;
        }
        public void RemoveClientFromGroup(string id)
        {
            var groupList = groups.ToList();
            for(int i = groupList.Count - 1; i >= 0; i--)
            {
                var users = groupList[i].Users;
                foreach(Client c in users)
                {
                    if (c.Connection_id.Equals(id))
                    {
                        context.Entry(c).State = EntityState.Deleted;
                        groupList[i].Users.Remove(c);
                        
                        if(groupList[i].Users.Count == 0)
                        {
                            this.Delete(groupList[i]);
                            break;
                        }
                        break;
                    }
                }
            }
        }
        public IQueryable<Group> FindAll()
        {
            return this.groups;
        }
        public void AddGroup(string grpname)
        {
            groups.Add(new Group() { Name = grpname, IsJoinable = true });
        }
        public Task<Group> FindByNameAsync(string name)
        {
          var grp = groups.Where(x => x.Name == name).FirstOrDefaultAsync();
          return grp;
        }
        public Group FindByName(string name)
        {
            return groups.Where(x => x.Name == name).FirstOrDefault();
        }
        public Group FindById(int id)
        {
            return groups.Find(id);
        }
        public void Delete(Group group)
        {
            groups.Remove(group);
        }
        public Task saveChangesAsync()
        {
           return context.SaveChangesAsync();
        }
        public void SaveChanges()
        {
            context.SaveChanges();
        }

    }
}