using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParentAgent.Models
{
    public class QuestionHistory
    {
        public QuestionHistory()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionHistoryId { get; set; }

        public Question Question { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength]
        public string QuestionTxt { get; set; }

        [MaxLength]
        public string Answer { get; set; }

        [MaxLength]
        public string OtherAnswer { get; set; }

        [Required]
        public Parent Parent { get; set; }

        [Required]
        public Student Student { get; set; }

        [Required]
        public Course Course { get; set; }

        public DateTime? DateAnswered { get; set; }

        public int NumOfTimesSentToTeacher { get; set; }
    }
}