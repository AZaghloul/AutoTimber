using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.DB.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ProfilePic { get; set; }
        public string About { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
