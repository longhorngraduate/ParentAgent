using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParentAgent.Models
{
    public class Error
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ErrorId { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }

        public string InnerException { get; set; }

        public string StackTrace { get; set; }

        public string Type { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateCreated { get; set; }
    }
}