﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParentAgent.Models
{
    public class ResponseCreateCourseViewModel
    {
        public CourseViewModel CourseVM { get; set; }

        public TeacherViewModel TeacherVM { get; set; }
    }
}