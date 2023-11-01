using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace College.Y.Models
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int TermId { get; set; }
        public string TermTitle { get; set; }
        public DateTime TermStart { get; set; }
        public DateTime TermEnd { get; set; }
        public int TermCourseCount { get; set; }
        public int CurrentTermId { get; internal set; }

    }
}
