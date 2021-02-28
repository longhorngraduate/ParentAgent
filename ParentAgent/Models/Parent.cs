using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParentAgent.Models
{
    public class Parent
    {
        public Parent()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParentId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

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
        [MaxLength(50)]
        public string Street1 { get; set; }

        [MaxLength(50)]
        public string Street2 { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [MaxLength(50)]
        public string State { get; set; }

        [Required]
        [MaxLength(50)]
        public string Zip { get; set; }

        [Required]
        public string NumOfChildren { get; set; }

        [Required]
        public List<Student> Children { get; set; }

        [Required]
        public int Active { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime? LastConnect { get; set; }

        //public List<QuestionHistory> QuestionHistorys { get; set; }


        //--------------- Not Mapped ---------------
        public string Address
        {
            get
            {
                string result = Street1;

                if (Street2 != null)
                {
                    if (Street2.Length > 0)
                        result += " " + Street2;
                }

                result += " " + City + ", " + State + " " + Zip;

                return result;
            }
        }

        public string PronounPossesion
        {
            get
            {
                if (Gender == 1)
                    return "his";
                else if (Gender == 2)
                    return "her";
                else
                    return "their";
            }
        }

        public string FullNameWithHonorific
        {
            get
            {
                string result = FirstName + " " + LastName;

                if (Gender == 1)
                    return "Mr. " + result;
                else if (Gender == 2)
                    return "Mrs. " + result;
                else
                    return result;
            }
        }

        public string FullNameWithHonorificWithPossesion
        {
            get
            {
                string lastNamePossesive = LastName;
                lastNamePossesive = lastNamePossesive.EndsWith("s") ? lastNamePossesive + "'" : lastNamePossesive + "'s";

                string result = FirstName + " " + lastNamePossesive;

                if (Gender == 1)
                    return "Mr. " + result;
                else if (Gender == 2)
                    return "Mrs. " + result;
                else
                    return result;
            }
        }

        public string Honorific
        {
            get
            {
                if (Gender == 1)
                    return "Mr.";
                else if (Gender == 2)
                    return "Mrs.";
                else
                    return "";
            }
        }
        //--------------- end of Not Mapped ---------------
    }


    public enum Gender
    {
        Male = 1,
        Female = 2,
        Other = 3
    }

    public enum ClassType
    {
        Regular = 1,
        PreAP = 2,
        AP = 3
    }

    public enum PreferredMethodOfContact
    {
        Email = 1,
        Text = 2,
        Phone = 3
    }
}