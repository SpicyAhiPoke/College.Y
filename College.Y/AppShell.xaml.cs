using College.Y.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace College.Y
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Terms), typeof(Terms));
            Routing.RegisterRoute(nameof(Courses), typeof(Courses));
            Routing.RegisterRoute(nameof(Assessments), typeof(Assessments));
            Routing.RegisterRoute(nameof(TermsInfo), typeof(TermsInfo));
            Routing.RegisterRoute(nameof(CoursesInfo), typeof(CoursesInfo));
            Routing.RegisterRoute(nameof(AssessmentsInfo), typeof(AssessmentsInfo));

        }

    }
}
