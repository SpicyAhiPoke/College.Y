using College.Y.Models;
using College.Y.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace College.Y.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Courses : ContentPage
    {
        public Courses()
        {
            InitializeComponent();

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            cvCourse.ItemsSource = await App.Database.GetCourses();
        }
        private async void tbiAdd_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CoursesInfo));
        }
        private async void cvCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                // Navigate to PageAdd, passing filename as query parameter.
                Course course = (Course)e.CurrentSelection.FirstOrDefault();
                var edit = new CoursesInfo(course)
                {
                    Title = (course.CourseName == null) ? "Add" : "Edit"
                };
                await Navigation.PushAsync(edit);
            }

        }
        private async void siDelete_Invoked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Delete", "Course?", "No", "Yes");
            if (!answer)
            {
                await DisplayAlert("Yes", "?", "OK");
            }
            else
            {
                await DisplayAlert("No", "?", "OK");

            }
        }
    }
}