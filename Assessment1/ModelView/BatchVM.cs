using Assessment1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment1.ModelView
{
    public class BatchVM
    {
        [Key]

        public string BatchId { get; set; }
        [Required(ErrorMessage = "BusinessUnit required")]
        public int BusinessUnitId { get; set; }
        public string BusinessUnit { get; set; }
        public string expiryDate { get; set; }
        public DateTime batchpublishedDate { get; set; }

        public static explicit operator BatchVM(List<object> v)
        {
            throw new NotImplementedException();
        }

        public acl acl { get; set; }
        //[ForeignKey("BatchId")]
        //public ACLreadUsers[] readUsers { get; set; }
        //[ForeignKey("BatchId")]
        //public ACLreadGroups[] readGroups { get; set; }
        [ForeignKey("BatchId")]
        public attributes[] attributes { get; set; }
    }
}
