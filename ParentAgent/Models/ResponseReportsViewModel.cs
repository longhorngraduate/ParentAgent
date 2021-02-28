using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParentAgent.Models
{
    public class ResponseReportsViewModel
    {
        public string NameOfReport { get; set; }
        
        public List<Parent> Parents { get; set; }

        public List<QuestionHistory> QuestionHistorys { get; set; }
    }
}