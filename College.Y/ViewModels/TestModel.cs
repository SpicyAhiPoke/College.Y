using College.Y.Models;
using College.Y.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace College.Y.ViewModels
{
    public class TestModel
    {
        public Term TestTerm { get; set; }
        public Course TestCourse { get; set; }
        public Assessment TestObjective { get; set; }
        public Assessment TestPerformance { get; set; }

        public TestModel()
        {
            var term = new Term
            {
                TermId = 1,
                TermTitle = "TestT",
                TermStart = DateTime.Now,
                TermEnd = DateTime.Now,
                TermCourseCount = 1,

            };
            TestTerm = term;
            var course = new Course
            {
                CourseId = 1,
                CourseName = "testC",
                CourseStatus = "PlanToTake",
                CourseStart = DateTime.Now,
                CourseEnd = DateTime.Now,
                NotificationStart = false,
                NotificationEnd = false,
                CourseNote = null,
                InstructorName = "Shawn Rainiel Caday-Ramos",
                InstructorPhone = "1234567890",
                InstructorEmail = "scadayr@wgu.edu",
                TermCourseId = 1,
                AssessmentCount = 2,

            };
            TestCourse = course;
            var objective = new Assessment
            {
                AssessmentId = 1,
                AssessmentName = "testO",
                AssessmentType = "Objective",
                AssessmentStart = DateTime.Now,
                AssessmentEnd = DateTime.Now,
                NotificationStart = false,
                NotificationEnd = false,
                CourseAssessmentId = 1,

            };
            TestObjective = objective;
            var performance = new Assessment
            {
                AssessmentId = 2,
                AssessmentName = "testP",
                AssessmentType = "Performance",
                AssessmentStart = DateTime.Now,
                AssessmentEnd = DateTime.Now,
                NotificationStart = false,
                NotificationEnd = false,
                CourseAssessmentId = 1,

            };
            TestPerformance = performance;
        }
    }
}
