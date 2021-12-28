using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment1.Models
{
    public class BatchFiles
    {
        public int Id { get; set; }
        public string BatchId { get; set; }
        public string fileName { get; set; }

        public string X_MIME_Type { get; set; }
        public decimal X_Content_Type { get; set; }
    }
}
