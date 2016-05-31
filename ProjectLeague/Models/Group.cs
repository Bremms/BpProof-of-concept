using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectLeague.Models
{
    [Table("Groups")]
    public class Group
    {
        public int Id { get; set; }
        public virtual List<Client> Users { get; set; }
        public string Name { get; set; }
        public bool IsJoinable { get; set; }
        public long Match_id { get; set; }

        public Group()
        {

        }
        public void AddUser(Client client)
        {
            this.Users.Add(client);
        }
    }
}