using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Portal.Web.Models.DataObject;

namespace Portal.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
    public class PersonalizationModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
 
        [Required(ErrorMessage = "Field can't be empty")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string Email { get; set; }
        public string Phone { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The must be 2 at least 6 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string RePassword { get; set; }

        public PersonalizationModel() { }
        public PersonalizationModel(UserProfile user)
        {
            Id = user.AspNetUser.Id;
            Firstname = user.Firstname;
            Lastname = user.Lastname;
            Email = user.Email;
            Phone = user.Phone;
            UserName = user.AspNetUser.UserName;
        }




    }


    public class EditCompanyModel
    {

        public int CompanyId { get; set; }

        public string isSendMail { get; set; }
       
        [Required(ErrorMessage = "Field can't be empty")]
        public string companyName { get; set; }

        [Display(Name = "Org Nummer")]
        public int orgNummer { get; set; }
        public string billinAddress { get; set; }
        public int zipCode { get; set; }

        public string city { get; set; }
 
        public string contry { get; set; }
      
        public string сompanyPhone { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]
        public string domain { get; set; }

        public string description { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]
        public string contactName { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]
        public string contactSurname { get; set; }

        public string contactPhone { get; set; }

        public string phone { get; set; }

        public string companyType { get; set; }

        public string headerColor { get; set; }

        public string backgroundColor { get; set; }

        public string menuActiveColor { get; set; }

        public string menuPassiveColor { get; set; }

        public string buttonsBackgroundColor { get; set; }

        public string buttonsActiveColor { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]  
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string email { get; set; }

        public string isChekedType(string val)
        {
            return (companyType != null && companyType == val) ? "checked" : "";
        }

        public string logoUrl { get; set; }

        public string loginLogoUrl { get; set; }

        public string startVidio { get; set; }
        
        public Company Company
        {
            get
            {
                var comanpy = new Company();
                UpdateCompany(comanpy);
                return comanpy;
            }
            set
            {
                domain = value.Domain;
                companyName = value.Name;
                companyType = value.CompanyType.ToString();
                zipCode = value.ZipCode??0;
                description = value.Description;
                city = value.City;
                phone = value.CompanyPhone;
                orgNummer = Convert.ToInt32(value.OrgNummer??"0");
                billinAddress = value.BillinAddress;
                contactName = value.ContactName;
                contactSurname = value.ContactSurname;
           
                contactPhone = value.ContactPhone;
                contry = value.Country;
                email = value.Email;
                CompanyId = value.CompanyId;
            }
        }

        public void UpdateCompany(Company company)
        {
            char last = domain[domain.Length - 1];
            if (last == '/')
            {
                domain = domain.Remove(domain.Length - 1, 1);
            }
            company.Domain = domain.Replace("http://","");
            company.CompanyId = CompanyId;
            company.CompanyPhone = phone;
            company.CompanyType = (CompanyType)Enum.Parse(typeof(CompanyType), companyType, true);
            company.Description = description;
            company.ContactName = contactName;
            company.ContactSurname = contactSurname;
            company.ContactPhone = contactPhone;
            company.BillinAddress = billinAddress;
            company.City = city;
            company.Name = companyName;
            company.OrgNummer = orgNummer.ToString();
            company.ZipCode = zipCode;
            company.Email = email;
            company.Country = contry;
        }
        public CustomizeCompany CustomizeCompany
        {
            get
            {
                return new CustomizeCompany(logoUrl, loginLogoUrl, headerColor, backgroundColor, buttonsBackgroundColor, buttonsActiveColor, menuActiveColor, menuPassiveColor, startVidio);
            }
            set
            {
                logoUrl = value.LogoUrl;
                loginLogoUrl = value.LoginLogoUrl;
                headerColor = value.HeaderColor;
                //  backgroundColor = value.BackgroundColor;
                buttonsBackgroundColor = value.ButtonsBackgroundColor;
                buttonsActiveColor = value.ButtonsActiveColor;
                menuActiveColor = value.MenuActiveColor;
                menuPassiveColor = value.MenuPassiveColor;
                startVidio = String.IsNullOrEmpty(value.StartVidio)? "": value.StartVidio;
            }
        }

    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
