using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParentAgent.Models
{
    public class Course
    {
        public Course()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(25)]
        public string TimeStart { get; set; }

        [Required]
        [MaxLength(25)]
        public string TimeEnd { get; set; }

        [Required]
        public int ClassType { get; set; }

        [Required]
        [MaxLength(100)]
        public string SchoolName { get; set; }

        [Required]
        public Teacher Teacher { get; set; }

        public List<QuestionHistory> QuestionHistorys { get; set; }

    }
    
}