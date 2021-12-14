using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment1.Models
{
    public class acl
    {
        public string[] readUsers { get; set; }
        public string[] readGroups { get; set; }
        //[ForeignKey("BatchId")]
        //public ACLreadUsers readUsers { get; set; }
        //[ForeignKey("BatchId")]
        //public ACLreadGroups readGroups { get; set; }

    }
}
