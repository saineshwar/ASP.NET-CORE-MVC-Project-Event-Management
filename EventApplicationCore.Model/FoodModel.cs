using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Model
{
    [NotMapped]
    public class FoodModel
    {
        public int FoodID { get; set; }
        public string FoodName { get; set; }

        [NotMapped]
        public bool FoodChecked { get; set; }
    }
}
