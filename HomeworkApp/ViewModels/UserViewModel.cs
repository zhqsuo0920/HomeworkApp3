using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HomeworkApp.ViewModels
{
    public class UserViewModel
    {
        [Key, Required]
        public string UserID { get; set; }
        [Required, DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required, DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required, DisplayName("Email")]
        public string EmailAddress { get; set; }
        public Nullable<System.DateTime> JoinedDate { get; set; }
    }
}