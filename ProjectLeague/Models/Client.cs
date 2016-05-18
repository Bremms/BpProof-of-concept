using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectLeague.Models
{
    [Table("Client")]
    public class Client
    {
        [Key]
        public string Connection_id { get; set; }

        public string Nickname { get; set; }

        public int Group_id { get; set; }
        [ForeignKey("Group_id")]
        public virtual Group Group { get; set; }
    }
}