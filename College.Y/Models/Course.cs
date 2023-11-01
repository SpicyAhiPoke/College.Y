using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace College.Y.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseStatus { get; set; }
        public DateTime CourseStart { get; set; }
        public DateTime CourseEnd { get; set; }
        public bool NotificationStart { get; set; }
        public bool NotificationEnd { get; set; }
        public string CourseNote { get; set; }
        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public int TermCourseId { get; set; }
        public int AssessmentCount { get; set;}
    }
}
