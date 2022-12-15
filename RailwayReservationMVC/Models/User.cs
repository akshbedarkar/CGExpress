using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace RailwayReservationMVC.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int User_Id { get; set; }

       
        public string FirstName { get; set; }

        
        public string LastName { get; set; }
        
        public string Gender { get; set; }
       
        public string Phone { get; set; }
        

        public string Email { get; set; }
        
        public string Password { get; set; }

        
        public string Role { get; set; }
    }
}