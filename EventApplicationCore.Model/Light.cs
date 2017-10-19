using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Model
{
    public class Light
    {
        [Key]
        public int LightID { get; set; }

        [Required(ErrorMessage = "Required Light Type")]
        public string LightType { get; set; }

        [Required(ErrorMessage = "Required Light Name")]
        public string LightName { get; set; }
        
        public string LightFilename { get; set; }

        public string LightFilePath { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter only numeric number")]
        [Required(ErrorMessage = "Required Light Cost")]
        public int? LightCost { get; set; }

        public int? Createdby { get; set; }
        public DateTime? Createdate { get; set; }

        [NotMapped]
        public string LightTypeName { get; set; }
    }
}
