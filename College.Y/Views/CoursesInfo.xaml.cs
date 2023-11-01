using College.Y.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Plugin.LocalNotification;
using System.Net.Mail;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace College.Y.Views
{
    [QueryProperty(nameof(CourseId), nameof(CourseId))]
    public partial class CoursesInfo : ContentPage
    {
        //initiation
        public CoursesInfo()
		{
			InitializeComponent ();
            //new course
            BindingContext = new Course();
            dpStart.Date = DateTime.Now;
            dpEnd.Date = DateTime.Now;

            //hide picker, collectionview, swipeitem
            pkAssessment.IsVisible = false;
            lbAssessment.Text = "Save to add Assessment(s).";
            cvCourseAssessment.IsVisible = false;
            edNote.Placeholder = "Save to Share";
            siShare.IsVisible = false;

        }
        public CoursesInfo(Course acourse)
        {
            InitializeComponent ();
            //term exists
            BindingContext = acourse;
            LoadAssessmentPicker();
            LoadCourseAssessment();

        }
        //generation
        public string CourseId { set { LoadCourse(value); } }
        private async void LoadCourse(string courseid)
        {
            try
            {
                int id = Convert.ToInt32(courseid);
                //retrieve + set as BC
                Course course = await App.Database.GetACourse(id);
                BindingContext = course;

            }
            catch (Exception)
            {
                
            }
        }
        private async void LoadCourseAssessment()
        {
            //get bind
            Course course = (Course)BindingContext;
            //assign int to var
            var current = course.CourseId;
            //assign list to source
            cvCourseAssessment.ItemsSource = await App.Database.GetCourseAssessments(current);

        }
        private async void LoadAssessmentPicker()
        {
            pkAssessment.ItemsSource = await App.Database.GetAssessments();

        }
        //function
        Func<StackLayout, bool> CheckNullEmpty = stl =>
        {
            bool IsNullEmpty = false;

            foreach (var ctrl in stl.Children)
            {
                switch (ctrl)
                {
                    case Label lb:
                    case Switch sw:
                    case Picker pk:
                    case DatePicker dp:
                    case Editor editor:
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
        //action
        private async Task ShareNote()
        {
            if (!string.IsNullOrEmpty(edNote.Text))
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Title = "Course Note",
                    Text = edNote.Text

                });

            }
            else
            {
                await AlertError();

            }
        }
        public async Task AlertError()
        {
            await DisplayAlert("Error", "Blank or Whitespace is invalid", "OK");

        }
        private async Task NotifyCourseStart()
        {

            var notify = new NotificationRequest
            {
                Title = "Course Start",
                Description = dpStart.Date.ToString("D"),
                Schedule =
                    {
                        NotifyTime = DateTime.Now.AddSeconds(5)

                    }
            };

            await LocalNotificationCenter.Current.Show(notify);

        }
        private async Task NotifyCourseEnd()
        {
            var notify = new NotificationRequest
            {
                Title = "Course End",
                Description = dpEnd.Date.ToString("D"),
                Schedule =
                    {
                        NotifyTime = DateTime.Now.AddSeconds(5)

                    }
            };

            await LocalNotificationCenter.Current.Show(notify);

        }
        //validation
        private async void IsLessThanTwo(Assessment pick, Course current)
        {
            if (cvCourseAssessment.ItemsSource is System.Collections.IList ilist)
            {
                current.AssessmentCount = ilist.Count;
                var count = current.AssessmentCount;
                bool oneobjective = ilist.OfType<Assessment>().Any(a => a.AssessmentType == "Objective");
                bool oneperformance = ilist.OfType<Assessment>().Any(a => a.AssessmentType == "Performance");
                Objective o = new Objective();
                Performance p = new Performance();

                if (count < 2)
                {
                    if (oneobjective)
                    {
                        o.ObjectiveCount++;

                    }
                    else if (oneperformance)
                    {
                        p.PerformanceCount++;

                    }

                    if ((!oneobjective && pick.AssessmentType == "Objective" || !oneperformance && pick.AssessmentType == "Performance"))
                    {
                        if (await DisplayAlert("Add?", $"Assessment: {pick.AssessmentName} \nto\nTerm: {current.CourseName}", "OK", "Cancel"))
                        {
                            await App.Database.AddCourseAssessment(pick);
                            cvCourseAssessment.ItemsSource = await App.Database.GetCourseAssessments(current.CourseId);
                        }
                    }
                    else
                    {
                        await DisplayAlert("Maximum!", $"1 assessment type each. \nObjective: {o.ObjectiveCount} \nPerformance: {p.PerformanceCount} ", "OK");

                    }
                }
                else
                {
                    await DisplayAlert("Maximum!", "2 assessments reached.", "OK");

                }
            }
        }
        private async Task IsSwitchToggled()
        {
            if (swStart.IsToggled && dpStart.Date >= DateTime.Today)
            {
                await NotifyCourseStart();

            }
            else if (swEnd.IsToggled && dpEnd.Date >= DateTime.Today)
            {
                await NotifyCourseEnd();
            }

        }
        private bool IsEmailValid(string email)
        {
            try
            {
                MailAddress address = new MailAddress(email);
                etyEmail.BackgroundColor = Color.White;
                return true;

            }
            catch (FormatException)
            {
                etyEmail.BackgroundColor = Color.PaleVioletRed;
                DisplayAlert("Error!", "Email invalid", "OK");
                return false;

            }
        }
        private bool IsPhoneValid(string phone)
        {
            if(phone.Length == etyPhone.MaxLength)
            {
                etyPhone.BackgroundColor = Color.White;
                return true;

            }
            else
            {
                etyPhone.BackgroundColor = Color.PaleVioletRed;
                DisplayAlert("Error!", "Phone invalid", "OK");
                return false;

            }
        }
        private bool IsContactValid(string phone, string email)
        {
            if (IsPhoneValid(phone) && IsEmailValid(email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //selection
        private void pkAssessment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is Picker picker && picker.SelectedItem is Assessment pick)
            {
                //cast bind to obj
                Course current = (Course)BindingContext;
                //assign pk to fk
                pick.CourseAssessmentId = current.CourseId;

                IsLessThanTwo(pick, current);

            }
        }
        private async void cvCourseAssessment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Assessment select = (Assessment)e.CurrentSelection.FirstOrDefault();
            var edit = new AssessmentsInfo(select)
            {
                Title = (select.AssessmentName == null) ? "Add" : "Edit"

            };
            await Navigation.PushAsync(edit);

        }
        //reaction
        private async void siShare_Invoked(object sender, EventArgs e)
        {
            await ShareNote();
        }
        private async void siRemove_Invoked(object sender, EventArgs e)
        {
            Course current = (Course)BindingContext;

            if (sender is SwipeItem swipe && swipe.BindingContext is Assessment select)
            {
                if (await DisplayAlert("Remove?", $"Assessment: {select.AssessmentName} \nfrom \nTerm: {current.CourseName}", "OK", "Cancel"))
                {
                    await App.Database.RemoveCourseAssessment(select);
                    LoadCourseAssessment();

                }
            }
        }
        private async void swStart_Toggled(object sender, ToggledEventArgs e)
        {
            await IsSwitchToggled();
        }
        private async void swEnd_Toggled(object sender, ToggledEventArgs e)
        {
            await IsSwitchToggled();
        }
        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            //bind model to obj
            var course = (Course)BindingContext;
            if (IsContactValid(etyPhone.Text, etyEmail.Text))
            {
                if (!CheckNullEmpty(slCoursesInfo))
                {
                    //add obj to db
                    await App.Database.AddCourse(course);
                    //back to
                    await Navigation.PopToRootAsync();

                }
                else
                {
                    await AlertError();

                }
            }
        }
        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            var course = (Course)BindingContext;
            await App.Database.DeleteCourse(course);
            await Shell.Current.GoToAsync("..");
        }
    }
}