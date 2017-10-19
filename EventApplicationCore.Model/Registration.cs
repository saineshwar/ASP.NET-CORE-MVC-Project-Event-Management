using System;
using System.ComponentModel.DataAnnotations;

namespace EventApplicationCore.Model
{
    public partial class Registration
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Name Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address Required")]
        public string Address { get; set; }

        [CityValidation]
        [Required(ErrorMessage = "City Required")]
        public int? City { get; set; }

        [StateValidation]
        [Required(ErrorMessage = "State Required")]
        public int? State { get; set; }

        [Required(ErrorMessage = "Country Required")]
        [CountryValidation]
        public int? Country { get; set; }

        [Required(ErrorMessage = "Mobileno Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Mobileno")]
        public string Mobileno { get; set; }

        [Required(ErrorMessage = "EmailID Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string EmailID { get; set; }

        [MinLength(6, ErrorMessage = "Minimum Username must be 6 in charaters")]
        [Required(ErrorMessage = "Username Required")]
        public string Username { get; set; }

        [MinLength(7, ErrorMessage = "Minimum Password must be 7 in charaters")]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Enter Valid Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Gender Required")]
        public string Gender { get; set; }

        public DateTime? Birthdate { get; set; }

        public int? RoleID { get; set; }

        public DateTime? CreatedOn { get; set; }
        
    }

    public class CountryValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string Message = string.Empty;
            if (Convert.ToString(value) == "0")
            {
                Message = "Choose Country";
                return new ValidationResult(Message);
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }

    public class StateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string Message = string.Empty;
            if (Convert.ToString(value) == "0")
            {
                Message = "State Required";
                return new ValidationResult(Message);
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }

    public class CityValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string Message = string.Empty;
            if (Convert.ToString(value) == "0")
            {
                Message = "City Required";
                return new ValidationResult(Message);
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
