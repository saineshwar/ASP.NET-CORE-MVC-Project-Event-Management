using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Model
{
    public partial class States
    {
        [Key]
        public int StateID { get; set; }
        public string StateName { get; set; }
        public int? CountryID { get; set; }
    }
}
