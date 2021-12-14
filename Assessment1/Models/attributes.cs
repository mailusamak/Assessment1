using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment1.Models
{
    public class attributes
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("BatchId")]
        public string BatchId { get; set; }
        [Required(ErrorMessage = "attributes key required")]
        public string key { get; set; }
        [Required(ErrorMessage = "value key required")]
        public string value { get; set; }
    }
}
