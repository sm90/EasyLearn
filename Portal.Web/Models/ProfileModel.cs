using System.ComponentModel.DataAnnotations;

namespace Portal.Web.Models
{
    [MetadataType(typeof(ProfileModel.Metadata))]
    public class ProfileModel
    {
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }
        public int DepartmentId { get; set; }
        public int CompanyId { get; set; }

        public bool IsAdministrator { get; set; }

        private sealed class Metadata
        {
            [RegularExpression("^([a-zA-Z0-9ØøÅåÆæ@.]){2,100}$", ErrorMessage = "Invalid username")]
            public string Username { get; set; }
            [Required(ErrorMessage = "Fornavn er obligatorisk")]
            [RegularExpression("^([a-zA-Z0-9ØøÅåÆæ]){2,100}$", ErrorMessage = "Invalid firstname")]
            public string Firstname { get; set; }
            [Required(ErrorMessage = "Etternavn er obligatorisk")]
            [RegularExpression("^([a-zA-Z0-9ØøÅåÆæ]){2,100}$", ErrorMessage = "Invalid lastname")]
            public string Lastname { get; set; }

            [Required(ErrorMessage = "Epost er obligatorisk")]
            public string Email { get; set; }

        }
    }
}