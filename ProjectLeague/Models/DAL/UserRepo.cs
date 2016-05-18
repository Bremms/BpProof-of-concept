using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ProjectLeague.Models.DAL
{
    public class UserRepo
    {
        private DbEntitiesContext ctx;
        private DbSet<Client> users;
        public UserRepo(DbEntitiesContext ctx)
        {
            this.ctx = ctx;
            this.users = ctx.Clients;
        }
        public void add(Client c)
        {
            users.Add(c);
        }
        public Client FindClientById(string connectionId)
        {
            return users.Find(connectionId);
        }
        public void Delete(Client c)
        {
            users.Remove(c);
        }
        public void addGroupToUser(string connection, Group grp)
        {
            users.Where(w => w.Connection_id == connection).FirstOrDefault().Group=grp;
        }
        public Task SaveChangesAsync()
        {
            return ctx.SaveChangesAsync();
        }
        public void SaveChanges()
        {
            ctx.SaveChanges();
        }
    }
}