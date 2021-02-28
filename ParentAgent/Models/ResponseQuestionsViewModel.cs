using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParentAgent.Models
{
    public class ResponseQuestionsViewModel
    {
        public List<QuestionHistory> QuestionHistorys { get; set; }

        public List<QuestionHistory> QuestionHistorysPending { get; set; }

        public QuestionHistoryViewModel QuestionHistoryVM { get; set; }

        public Parent Parent { get; set; }

        public Student Student { get; set; }

        public Course Course { get; set; }
    }
}