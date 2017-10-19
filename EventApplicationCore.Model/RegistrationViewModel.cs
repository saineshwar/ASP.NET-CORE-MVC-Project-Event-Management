using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Model
{
    [NotMapped]
    public class RegistrationViewModel
    {

        public int ID { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string CityName { get; set; }

        public string StateName { get; set; }

        public string CountryName { get; set; }

        public string Mobileno { get; set; }

        public string EmailID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Gender { get; set; }

        public string Birthdate { get; set; }

        public int? RoleID { get; set; }

        public string CreatedOn { get; set; }
    }
}
