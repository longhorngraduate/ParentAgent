﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParentAgent.Models
{
    public class Student
    {
        public Student()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }

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
        public List<Course> Courses { get; set; }

        [Required]
        public List<Teacher> Teachers { get; set; }

        //[Required]
        //public List<QuestionHistory> WeeklyQuestions { get; set; }

        //[Required]
        //public List<QuestionHistory> QuestionHistorys { get; set; }

        [Required]
        public int Active { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime? LastActive { get; set; }


        //--------------- Not Mapped ---------------
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        //--------------- end of Not Mapped ---------------
    }
}