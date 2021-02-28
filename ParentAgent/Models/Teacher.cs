using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParentAgent.Models
{
    public class Teacher
    {
        public Teacher()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeacherId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public int Gender { get; set; }

        [Required]
        [MaxLength(50)]
        public string Phone { get; set; }

        [Required]
        public int PreferredMethodOfContact { get; set; }

        [NotMapped]
        public string Honorific
        {
            get
            {
                if (Gender == 1)
                    return "Mr.";
                else if (Gender == 2)
                    return "Mrs.";
                else
                    return "Professor";
            }
            //no set-accessor
        }
    }
    
}