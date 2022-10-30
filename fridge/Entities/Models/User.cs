using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "FirstName is required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length of FirstName is 30 characters.")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "LastName is required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length of LastName is 30 characters.")]
        public string LastName { get; set; }
    }
}
