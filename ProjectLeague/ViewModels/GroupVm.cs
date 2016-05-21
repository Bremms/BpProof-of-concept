using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectLeague.ViewModels
{
    public class GroupVm
    {
        public string Name { get; set; }
        public int UserCount { get; set; }
        public long MatchId { get; set; }
        public GroupVm()
        {

        }
    }
}