using College.Y.Models;
using College.Y.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace College.Y.Services
{
    public class SQLiteHelper
    {
        private readonly SQLiteAsyncConnection connection;
        //creation
        public SQLiteHelper(string dbPath)
        {
            connection = new SQLiteAsyncConnection(dbPath);
            connection.DropTableAsync<Course>().Wait();
            connection.DropTableAsync<Term>().Wait();
            connection.DropTableAsync<Assessment>().Wait();

            connection.CreateTableAsync<Course>().Wait();
            connection.CreateTableAsync<Term>().Wait();
            connection.CreateTableAsync<Assessment>().Wait();
            TestModel t = new TestModel();
            connection.InsertAsync(t.TestCourse).Wait();
            connection.InsertAsync(t.TestTerm).Wait();
            connection.InsertAsync(t.TestObjective).Wait();
            connection.InsertAsync(t.TestPerformance).Wait();

        }

        //term
        public Task<List<Term>> GetTerms()
        {
            //list creation
            return connection.Table<Term>().ToListAsync();

        }
        public Task<Term> GetATerm(int id)
        {
            // specific term
            return connection.Table<Term>()
                            .Where(i => i.TermId == id)
                            .FirstOrDefaultAsync();
        }
        public Task AddTerm(Term term)
        {
            //save
            if (term.TermId != 0)
            {
                //update existing
                return connection.UpdateAsync(term);
            }
            else
            {
                //save new
                return connection.InsertAsync(term);
            }
        }
        public Task<int> DeleteTerm(Term term)
        {
            //Delete
            return connection.DeleteAsync(term);
        }
        //termcourse
        public Task<List<Course>> GetTermCourses(int termid)
        {
            //query courses of term
            return connection.Table<Course>()
                            .Where(i => i.TermCourseId == termid)
                            .ToListAsync();
        }
        public Task AddTermCourse(Course newcourse)
        {
            if (newcourse.TermCourseId != 0)
            {
                //update existing
                return connection.UpdateAsync(newcourse);

            }
            else
            {
                //save new
                return connection.InsertAsync(newcourse);

            }
        }
        public Task RemoveTermCourse(Course select)
        {
            //dissassociate
            select.TermCourseId = 0;

            //update db
            return connection.UpdateAsync(select);

        }
        //course
        public Task<List<Course>> GetCourses()
        {
            //list creation
            return connection.Table<Course>().ToListAsync();
        }
        public Task<Course> GetACourse(int id)
        {
            // specific course
            return connection.Table<Course>()
                            .Where(i => i.CourseId == id)
                            .FirstOrDefaultAsync();
        }
        public Task<int> AddCourse(Course course)
        {
            //save
            if (course.CourseId != 0)
            {
                //update existing
                return connection.UpdateAsync(course);
            }
            else
            {
                //save new
                return connection.InsertAsync(course);
            }
        }
        public Task<int> DeleteCourse(Course course)
        {
            //Delete
            return connection.DeleteAsync(course);
        }
        //assessment
        public Task<List<Assessment>> GetAssessments()
        {
            //create list from table
            return connection.Table<Assessment>().ToListAsync();
        }
        public Task<Assessment> GetNAssessment(int id)
        {
            //get by id
            return connection.Table<Assessment>()
                            .Where(i => i.AssessmentId == id)
                            .FirstOrDefaultAsync();
        }
        public Task<int> AddAssessment(Assessment assess)
        {
            //save
            if (assess.AssessmentId != 0)
            {
                //update existing
                return connection.UpdateAsync(assess);
            }
            else
            {
                //save new
                return connection.InsertAsync(assess);
            }
        }
        public Task<int> DeleteAssessment(Assessment assess)
        {
            //Delete
            return connection.DeleteAsync(assess);
        }
        //courseassessment
        public Task<List<Assessment>> GetCourseAssessments(int courseid)
        {
            return connection.Table<Assessment>()
                .Where(i => i.CourseAssessmentId == courseid)
                .ToListAsync();

        }
        public Task AddCourseAssessment(Assessment newassessment)
        {
            if (newassessment.CourseAssessmentId != 0)
            {
                //update
                return connection.UpdateAsync(newassessment);
            }
            else
            {
                //save new
                return connection.InsertAsync(newassessment);
            }

        }
        public Task RemoveCourseAssessment(Assessment select)
        {
            //dissassociate
            select.CourseAssessmentId = 0;

            //update db
            return connection.UpdateAsync(select);

        }

    }
}
