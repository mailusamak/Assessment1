﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment1.Models
{
    public class Batch
    {
        [Key]
        public string BatchId { get; set; }
        [Required]
        public string BusinessUnit { get; set; }
        public string expiryDate { get; set; }
        public DateTime batchpublishedDate { get; set; }
    }
}
