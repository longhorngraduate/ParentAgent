using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParentAgent.Models
{
    public class ResponseMainViewModel
    {
        public Parent Parent { get; set; }

        public Student Student { get; set; }

        public Course Course { get; set; }

        public Teacher Teacher { get; set; }

        public Question Question { get; set; }

        public QuestionHistory QuestionHistory { get; set; }
    }
}