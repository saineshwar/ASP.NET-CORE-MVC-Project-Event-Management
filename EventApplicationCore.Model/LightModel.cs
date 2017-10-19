using System.ComponentModel.DataAnnotations.Schema;

namespace EventApplicationCore.Model
{
    public class LightModel
    {
        public int LightID { get; set; }
        public string LightName { get; set; }

        [NotMapped]
        public bool LightChecked { get; set; }
    }
}
