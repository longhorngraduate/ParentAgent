using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParentAgent.Models
{
    public class QuestionHistoryViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionHistoryId { get; set; }

        public Question Question { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string QuestionTxt { get; set; }

        [MaxLength]
        public string Answer { get; set; }

        [MaxLength]
        public string OtherAnswer { get; set; }

        public DateTime? DateAnswered { get; set; }

        public int ParentId { get; set; }

        public int StudentId { get; set; }

        public int CourseId { get; set; }

        public Parent Parent { get; set; }

        public Student Student { get; set; }

        public Course Course { get; set; }
    }
}