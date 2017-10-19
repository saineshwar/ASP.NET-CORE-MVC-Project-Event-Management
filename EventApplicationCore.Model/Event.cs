using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Model
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }
        public string EventType { get; set; }
        public string Status { get; set; }
    }
}
