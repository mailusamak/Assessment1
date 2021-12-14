using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment1.Models
{
    public class ACLreadGroups
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string readGroups { get; set; }
        [Required]
        [ForeignKey("BatchId")]
        public string BatchId { get; set; }
    }
}
