using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HomeworkApp.ViewModels
{
    public class UserListViewModel
    {
        public string UserID { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }
        [DisplayName("Joined")]
        public Nullable<System.DateTime> JoinedDate { get; set; }
    }
}