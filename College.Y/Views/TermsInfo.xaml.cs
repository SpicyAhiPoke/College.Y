using College.Y.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using College.Y.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace College.Y.Views
{
    [QueryProperty(nameof(TermId), nameof(TermId))]
    public partial class TermsInfo : ContentPage
    {
        //initiation
        public TermsInfo()
		{
			InitializeComponent ();
			//new term
			BindingContext = new Term();
            dpStart.Date = DateTime.Now;
            dpEnd.Date = DateTime.Now;
            pkCourse.IsVisible = false;
            lbCourse.Text = "Save to add Course(s).";
            cvTermCourse.IsVisible = false; 
        }
        public TermsInfo(Term aterm)
        {
            InitializeComponent();
            //assign object to bind
            BindingContext = aterm;
            LoadCoursePicker();
            LoadTermCourse();

        }
        //generation
        public string TermId { set { LoadTerm(value); } }
        private async void LoadCoursePicker()
        {
            pkCourse.ItemsSource = await App.Database.GetCourses();
        }
        private async void LoadTerm(string termid)
        {
            try
            {
                int id = Convert.ToInt32(termid);
                //retrieve + set as BC
                Term term = await App.Database.GetATerm(id);
                BindingContext = term;
            }
            catch (Exception)
            {

            }
        }
        private async void LoadTermCourse()
        {
            //get bind
            Term term = (Term)BindingContext;
            //assign int to var
            var current = term.TermId;
            //assign list to source
            cvTermCourse.ItemsSource = await App.Database.GetTermCourses(current);

        }
        //function
        Func<StackLayout, bool> CheckNullEmpty = sl =>
        {
            bool IsNullEmpty = false;

            foreach (var ctl in sl.Children)
            {
                switch (ctl)
                {
                    case Label lb:
                    case Picker pk:
                    case DatePicker dp:
                        continue;
                    case Entry entry:
                        if (string.IsNullOrEmpty(entry.Text))
                        {
                            entry.BackgroundColor = Color.PaleVioletRed;
                            IsNullEmpty = true;

                        }
                        else
                        {
                            entry.BackgroundColor = Color.White;

                        }
                        break;
                    default:
                        break;
                }
            }
            return IsNullEmpty;
        };
        //validation
        private async void IsLessThanSix(Course select, Term current)
        {
            if (cvTermCourse.ItemsSource is System.Collections.IList ilist)
            {
                //get list count
                current.TermCourseCount = ilist.Count;
                //assign count to obj
                var count = current.TermCourseCount;

                if (count < 6)
                {
                    if (await DisplayAlert("Add?", $"Course: {select.CourseName} \nto\nTerm: {current.TermTitle}", "OK", "Cancel"))
                    {
                        //add select to db
                        await App.Database.AddTermCourse(select);
                        //assign list to source
                        cvTermCourse.ItemsSource = await App.Database.GetTermCourses(current.TermId);

                    }
                }
                else
                {
                    await DisplayAlert("Maximum!", "6 courses reached.", "OK");

                }
            }
        }
        //action
        public async Task AlertError()
        {
            await DisplayAlert("Error", "Blank or Whitespace is invalid", "OK");

        }
        //selection
        private void pkCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is Picker picker && picker.SelectedItem is Course pick)
            {
                //cast bind to obj
                Term current = (Term)BindingContext;
                //assign pk to fk
                pick.TermCourseId = current.TermId;
                //validation
                IsLessThanSix(pick, current);

            }
        }
        private async void cvTermCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //cast select to obj
            var select = (Course)e.CurrentSelection.FirstOrDefault();
            //cast bind to object
            var current = (Term)BindingContext;

            if (await DisplayAlert("Remove?", $"Course: {select.CourseName} \nfrom \nTerm: {current.CurrentTermId}", "OK", "Cancel"))
            {
                if (e.CurrentSelection != null)
                {
                    //remove from list
                    await App.Database.RemoveTermCourse(select);
                    //load to list
                    LoadTermCourse();

                }
            }
        }
        //reaction
        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            //cast bind to object
            var aterm = (Term)BindingContext;
            if (!CheckNullEmpty(slTermsInfo))
            {
                //add to db
                await App.Database.AddTerm(aterm);
                //back to main
                await Navigation.PopToRootAsync();

            }
            else
            {
                await AlertError();

            }
        }
        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            //cast bind to object
            var aterm = (Term)BindingContext;
            //delete from db
            await App.Database.DeleteTerm(aterm);
            //back to main
            await Shell.Current.GoToAsync("..");

        }
    }
}