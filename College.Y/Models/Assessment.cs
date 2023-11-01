using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace College.Y.Models
{
    public  class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public string AssessmentType { get; set; }
        public DateTime AssessmentStart { get; set; }
        public DateTime AssessmentEnd { get; set; }
        public bool NotificationStart { get; set; }
        public bool NotificationEnd { get; set; }
        public int CourseAssessmentId { get; set;}
    }

    public class Objective : Assessment 
    {
        public int ObjectiveCount { get; set;}
    }

    public class Performance : Assessment
    {
        public int PerformanceCount { get; set;}
    }
}
